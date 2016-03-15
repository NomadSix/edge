using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.Backing;
using Lidgren.Network;
using Edge.NetCommon;
using Edge.Hyperion.UI.Implementation.Popups;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Microsoft.Xna.Framework.Input;

namespace Edge.Hyperion.Engine {
    public class Town : Screen {
        Texture2D pixel, artDebug;
        NetClient atlasClient;
        string Port, Address;
        string Name;
        Color pColor = Color.Gray;

        TileMap Map = new TileMap(@"Map\grassDemo.csv");
        Point mapSize = new Point(AssetStore.TownSize);
        Vector2 MoveVector;

        MouseState old, newer;

        List<Slider> colorSliders = new List<Slider>();

        PauseMenu pMenu;

        Rectangle Portal;

        List<DebugPlayer> players = new List<DebugPlayer>();
        int currentFrame = 0;
        int totalFrames = 2;
        int framesPerRow = 2;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 200;

        public Town(Game game, string address, string port) : base(game) {
            Port = port;
            Address = address;
            cam.Zoom = 2f;
        }

        public override void Initialize() {
            #region Atlas Configuration
            var atlasConfig = new NetPeerConfiguration("Atlas");
            //maestroConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            atlasConfig.Port = 2347;
            atlasClient = new NetClient(atlasConfig);
            atlasClient.Start();
            atlasClient.Connect(Address, int.Parse(Port));
            #endregion
            that.sampleState = SamplerState.PointClamp;
            Portal = new Rectangle(AssetStore.TileSize * 0, AssetStore.TileSize * 10, 32, 32);
            pMenu = new PauseMenu(that, this);
            that.Components.Add(pMenu);
            base.Initialize();
        }

        protected override void LoadContent() {
            artDebug = that.Content.Load<Texture2D>(@"..\Images\Sheets\Player\MageWalkingSpriteColor.png");
            Tile.TileSetTexture = that.Content.Load<Texture2D>(@"..\Images\Sheets\Tiles\GrassSheet");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            if (_isActive == false) {
                atlasClient.Disconnect("Disconnecting");
                atlasClient.Shutdown("Shutingdown");
            }

            if (pMenu._isActive == false) {
                cameraControl();
                playerMovment(gameTime);
            }

            if (that.kb.IsButtonToggledDown(Keys.Space)) {
                pColor.R = (byte)AssetStore.rng.Next(0, 255);
                pColor.G = (byte)AssetStore.rng.Next(0, 255);
                pColor.B = (byte)AssetStore.rng.Next(0, 255);
            }
            serverIO();

            if (pMenu._isActive) {
                MoveVector = Vector2.Zero;
            }

            foreach (var player in players.Where(x => x.NetID == atlasClient.UniqueIdentifier)) {
                cam.Position = new Vector2(player.Location.X - viewport.Width / 2, player.Location.Y - viewport.Height / 2);
            }
            pMenu.update(new Vector2(cam.Position.X - 50f + viewport.Width / 2.0f + 16, cam.Position.Y - 50f + viewport.Height / 2.0f + 16), cam);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            Point firstPos = new Point(mapSize.X / 2 * AssetStore.TileSize);
            for (int y = 0; y < mapSize.Y; y++) {
                for (int x = 0; x < mapSize.X; x++) {
                    Rectangle rec = new Rectangle((x * AssetStore.TileSize) - firstPos.X, (y * AssetStore.TileSize) - firstPos.Y, AssetStore.TileSize, AssetStore.TileSize);
                    Rectangle sourceRec = Tile.GetScorceRectangle(int.Parse(Map.Rows[y][x]));
                    that.batch.Draw(Tile.TileSetTexture, rec, sourceRec, Color.White);
                }
            }
            that.batch.Draw(AssetStore.Pixel, Portal, Color.White);

            foreach (var p in players) {
                var mouse = Vector2.Transform(that.mouse.LocationV2, cam.Deproject);
                var scale = 0.25f;
                var mesurments = that.Helvetica.MeasureString(p.Name);
                var location = new Vector2((p.Location.X + 16) - mesurments.X / 2 * scale, p.Location.Y - 32 + 16);
                that.batch.DrawString(that.Helvetica, pMenu._isActive ? string.Empty : p.Name, location, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                that.batch.Draw(artDebug, p.Location, new Rectangle((currentFrame % framesPerRow) * 32, 0, 32, 32), new Color(p.R, p.G, p.B), 0f, Vector2.Zero, new Vector2(1f), p.MoveVector.X > 0.1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                that.batch.Draw(AssetStore.Pixel, p.AttackRec, Color.Wheat);
            }
            pMenu.draw(new Vector2(cam.Position.X - 50f + viewport.Width / 2.0f + 16, cam.Position.Y - 50f + viewport.Height / 2.0f + 16));
            DrawMouse();
        }

        public void serverIO() {
            NetOutgoingMessage outMsg = atlasClient.CreateMessage();
            outMsg.Write((byte)AtlasPackets.RequestPositionChange);
            outMsg.Write((short)MoveVector.X);
            outMsg.Write((short)MoveVector.Y);
            outMsg.Write(pColor.R);
            outMsg.Write(pColor.G);
            outMsg.Write(pColor.B);
            outMsg.Write(Environment.UserName);
            atlasClient.SendMessage(outMsg, NetDeliveryMethod.ReliableSequenced);

            NetIncomingMessage inMsg;
            while ((inMsg = atlasClient.ReadMessage()) != null) {
                switch (inMsg.MessageType) {
                    case NetIncomingMessageType.Data:
                        switch ((AtlasPackets)inMsg.ReadByte()) {
                            //Would this work? We need a way for the clients to know there own id's
                            //case AtlasPackets.FirstID:
                            //    NetID = inMsg.ReadInt64();
                            //    break;
                            case AtlasPackets.UpdatePositions:
                                players.Clear();
                                ushort numPlayers = inMsg.ReadUInt16();
                                for (ushort i = 0; i < numPlayers; i++)
                                    players.Add(new DebugPlayer(that, inMsg.ReadInt64(), inMsg.ReadSingle(), inMsg.ReadSingle(), inMsg.ReadByte(), inMsg.ReadByte(), inMsg.ReadByte(), inMsg.ReadString()));
                                break;
                            case AtlasPackets.UpdateMoveVector:
                                numPlayers = inMsg.ReadUInt16();
                                for (ushort i = 0; i < numPlayers; i++)
                                    players[i].MoveVector = new Vector2(inMsg.ReadSingle(), inMsg.ReadSingle());
                                break;
                        }
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        atlasClient.Connect(inMsg.SenderEndPoint);
                        break;
                }
            }
        }

        public void playerMovment(GameTime gameTime) {

            if (that.kb.IsButtonDown(Keys.W) || that.kb.IsButtonDown(Keys.Up)) {
                MoveVector.Y = -1;
            } else if (that.kb.IsButtonDown(Keys.S) || that.kb.IsButtonDown(Keys.Down)) {
                MoveVector.Y = 1;
            } else {
                MoveVector.Y = 0;
            }

            if (that.kb.IsButtonDown(Keys.A) || that.kb.IsButtonDown(Keys.Left)) {
                MoveVector.X = -1;
            } else if (that.kb.IsButtonDown(Keys.D) || that.kb.IsButtonDown(Keys.Right)) {
                MoveVector.X = 1;
            } else {
                MoveVector.X = 0;
            }
            
            
            if (MoveVector != Vector2.Zero) {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame) {
                    timeSinceLastFrame -= millisecondsPerFrame;
                    currentFrame += 1;
                }
            }
        }

        public void cameraControl() {
            var zoomFactor = 0.3f;
            newer = Mouse.GetState();
            if (newer.ScrollWheelValue > old.ScrollWheelValue) {
                cam.Zoom += cam.Zoom <= 3 ? zoomFactor : 0;
            } else if (newer.ScrollWheelValue < old.ScrollWheelValue) {
                cam.Zoom -= cam.Zoom >= 2 ? zoomFactor : 0;
            } else if (that.kb.IsButtonDown(Keys.C) || that.kb.IsButtonDown(Keys.Home)) {
                cam.Zoom = 1.0f;
            }
            old = newer;
        }
    }
}
