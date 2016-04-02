using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using Edge.Hyperion.UI.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

using Edge.Hyperion.Backing;
using Edge.NetCommon;
using Edge.Hyperion.UI.Implementation.Popups;

namespace Edge.Hyperion.Engine {
    public class Town : Screen {
        Texture2D pixel, artDebug;
        NetClient atlasClient;
        string Port, Address;
        string Name;
        Color pColor = Color.Gray;
        float health = 1;

        TileMap Map = new TileMap(@"Map\grassDemo.csv");
        Point mapSize = new Point(AssetStore.TownSize);
        Vector2 MoveVector;

        MouseState old, newer;

        PauseMenu pMenu;
        StatusBar statusBar;

        List<DebugPlayer> players = new List<DebugPlayer>();
        List<Enemy> enemys = new List<Enemy>();

        int currentFrame = 0;
        int framesPerRow = 2;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 200;
        int mult = 0;

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
            pMenu = new PauseMenu(that, this);
            statusBar = new StatusBar(that, 1f);
            that.Components.Add(statusBar);
            that.Components.Add(pMenu);
            base.Initialize();
        }

        protected override void LoadContent() {
            artDebug = AssetStore.PlayerTexture;
            Tile.TileSetTexture = that.Content.Load<Texture2D>(@"..\Images\Sheets\Tiles\GrassSheet");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            if (_isActive == false) {
                atlasClient.Disconnect("Disconnecting");
                atlasClient.Shutdown("Shutingdown");
            }

            if (AssetStore.kb.IsButtonToggledDown(Keys.Space)) {
                health = 1f;
            }
            serverIO();
            foreach (var player in players.Where(x => x.NetID == atlasClient.UniqueIdentifier)) {
                if (pMenu._isActive == false) {
                    cameraControl();
                    playerMovment(gameTime, player);
                }

                cam.Position = new Vector2(player.X - viewport.Width / 2, player.Y - viewport.Height / 2);
                foreach (var e in enemys.Where(x => x.isActive == true)) {
                    if (player.HitBox.Intersects(e.hitBox))
                        health -= .005f;
                    if (e.hitBox.Intersects(player.AttackRec))
                        e.Health -= .0025f;
                }
                player.Health = health;
            }

            if (pMenu._isActive) {
                MoveVector = Vector2.Zero;
            }
            pMenu.update(new Vector2(cam.Position.X - 50f + viewport.Width / 2.0f + 16, cam.Position.Y - 50f + viewport.Height / 2.0f + 16), cam);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            Point firstPos = new Point(mapSize.X / 2 * AssetStore.TileSize);
            for (int y = 0; y < mapSize.Y; y++) { // Main loop to draw the tile map .csv to the world
                for (int x = 0; x < mapSize.X; x++) {
                    Rectangle rec = new Rectangle((x * AssetStore.TileSize) - firstPos.X, (y * AssetStore.TileSize) - firstPos.Y, AssetStore.TileSize, AssetStore.TileSize);
                    Rectangle sourceRec = Tile.GetScorceRectangle(int.Parse(Map.Rows[y][x]));
                    that.batch.Draw(Tile.TileSetTexture, rec, sourceRec, Color.White);
                }
            }

            foreach (var p in players) { // Main loop to draw every player that is connected to the server
                var mouse = Vector2.Transform(AssetStore.mouse.LocationV2, cam.Deproject);
                var scale = 0.25f;
                var mesurments = that.Helvetica.MeasureString(p.Name);
                var location = new Vector2((p.X + p.Width / 2) - mesurments.X / 2 * scale, p.Y - p.Width / 2);
                that.batch.DrawString(that.Helvetica, pMenu._isActive ? string.Empty : p.Name, location, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                that.batch.Draw(artDebug, p.HitBox, new Rectangle((currentFrame % framesPerRow) * p.Width, mult * p.Width, p.Width, p.Width), Color.White);
                that.batch.Draw(AssetStore.Pixel, new Vector2(p.X, p.Y), null, Color.Red, (float)System.Math.PI, Vector2.Zero, new Vector2(2, 100 * p.Health), SpriteEffects.None, 0f);
                that.batch.Draw(AssetStore.Pixel, p.HitBox, new Color(Color.Red, 100));
            }
            foreach (var e in enemys) { // Main loop to draw each enemy to the world
                that.batch.Draw(AssetStore.Pixel, e.hitBox, Color.Red);
            }
            statusBar.draw(health);
            pMenu.draw(new Vector2(cam.Position.X - 50f + viewport.Width / 2.0f + 16, cam.Position.Y - 50f + viewport.Height / 2.0f + 16));
        }

        public void serverIO() {
            NetOutgoingMessage outMsg = atlasClient.CreateMessage();
            outMsg.Write((byte)AtlasPackets.RequestPositionChange);
            outMsg.Write((short)MoveVector.X);
            outMsg.Write((short)MoveVector.Y);
            outMsg.Write(pColor.R);
            outMsg.Write(pColor.G);
            outMsg.Write(pColor.B);
            outMsg.Write(System.Environment.UserName);
            outMsg.Write(health);
            atlasClient.SendMessage(outMsg, NetDeliveryMethod.ReliableSequenced);

            foreach (Enemy e in enemys) {
                outMsg = atlasClient.CreateMessage();
                outMsg.Write((byte)AtlasPackets.RequestMoveVector);
                outMsg.Write(e.NetID);
                outMsg.Write(e.Target.X);
                outMsg.Write(e.Target.Y);
                atlasClient.SendMessage(outMsg, NetDeliveryMethod.ReliableSequenced);
            }

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
                                    players.Add(new DebugPlayer(inMsg.ReadInt64(), inMsg.ReadInt32(), inMsg.ReadInt32(), inMsg.ReadByte(), inMsg.ReadByte(), inMsg.ReadByte(), inMsg.ReadString(), inMsg.ReadFloat()));
                                break;
                            case AtlasPackets.UpdateMoveVector:
                                if (enemys.Count != 0) {
                                    int ID = inMsg.ReadInt32();
                                    int X = inMsg.ReadInt32();
                                    int Y = inMsg.ReadInt32();
                                    enemys[ID - 1].Target = new Point(X, Y);
                                }
                                break;
                            case AtlasPackets.DamageEnemy:
                                enemys.Clear();
                                ushort numEnemys = inMsg.ReadUInt16();
                                for (ushort i = 0; i < numEnemys; i++) {
                                    enemys.Add(new Enemy(inMsg.ReadInt64(), inMsg.ReadInt32(), inMsg.ReadInt32()));
                                }
                                break;
                        }
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        atlasClient.Connect(inMsg.SenderEndPoint);
                        break;
                }
            }
        }

        public void cameraControl() {
            var zoomFactor = 0.3f;
            newer = Microsoft.Xna.Framework.Input.Mouse.GetState();
            if (newer.ScrollWheelValue > old.ScrollWheelValue) {
                cam.Zoom += cam.Zoom <= 3 ? zoomFactor : 0;
            } else if (newer.ScrollWheelValue < old.ScrollWheelValue) {
                cam.Zoom -= cam.Zoom >= 2 ? zoomFactor : 0;
            } else if (AssetStore.kb.IsButtonDown(Keys.C) || AssetStore.kb.IsButtonDown(Keys.Home)) {
                cam.Zoom = 1.0f;
            }
            old = newer;
        }

        public void playerMovment(GameTime gameTime, DebugPlayer p) {
            if (AssetStore.kb.IsButtonDown(Keys.W) || AssetStore.kb.IsButtonDown(Keys.Up)) {
                mult = 2;
                MoveVector.Y = -1;
            } else if (AssetStore.kb.IsButtonDown(Keys.S) || AssetStore.kb.IsButtonDown(Keys.Down)) {
                mult = 1;
                MoveVector.Y = 1;
            } else {
                MoveVector.Y = 0;
            }

            if (AssetStore.kb.IsButtonDown(Keys.A) || AssetStore.kb.IsButtonDown(Keys.Left)) {
                mult = 0;
                MoveVector.X = -1;
            } else if (AssetStore.kb.IsButtonDown(Keys.D) || AssetStore.kb.IsButtonDown(Keys.Right)) {
                mult = 3;
                MoveVector.X = 1;
            } else {
                MoveVector.X = 0;
            }

            if (MoveVector != Vector2.Zero) {
                timeSinceLastFrame += (byte)gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame) {
                    timeSinceLastFrame -= millisecondsPerFrame;
                    currentFrame += 1;
                }
            } else {
                currentFrame = 0;
            }
        }
    }
}
