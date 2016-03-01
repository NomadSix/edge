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
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Edge.Hyperion.Engine {
    public class Town : Screen {
        Texture2D pixel, artDebug;
        NetClient atlasClient;
        string Port, Address;

        TileMap Map = new TileMap(@"Map\grassDemo.csv");
        Point mapSize = new Point(100);
        Vector2 MoveVector;

        List<DebugPlayer> players = new List<DebugPlayer>();
        int currentFrame = 0;
        int totalFrames = 2;
        int framesPerRow = 2;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 200;

        public Town(Game game, String address, String port) : base(game) {
            Port = port;
            Address = address;
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
            base.Initialize();
        }

        protected override void LoadContent() {
            artDebug = that.Content.Load<Texture2D>(@"..\Images\Sheets\Player\MageWalkingSprite.png");
            Tile.TileSetTexture = that.Content.Load<Texture2D>(@"..\Images\Sheets\Tiles\GrassSheet");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            cameraControl();
            playerMovment(gameTime);
            serverIO();

            foreach (var player in players.Where(x => x.NetID == atlasClient.UniqueIdentifier)) {
                cam.Position = new Vector2(player.Location.X - viewport.Width / 2, player.Location.Y - viewport.Height / 2);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            Point firstPos = new Point(mapSize.X/2 * AssetStore.TileSize);
            for (int y = 0; y < mapSize.Y; y++) {
                for(int x = 0; x < mapSize.X; x++) {
                    Rectangle rec = new Rectangle((x * AssetStore.TileSize) - firstPos.X, (y * AssetStore.TileSize) - firstPos.Y, AssetStore.TileSize, AssetStore.TileSize);
                    Rectangle sourceRec = Tile.GetScorceRectangle(int.Parse(Map.Rows[y][x]));
                    that.batch.Draw(Tile.TileSetTexture, rec, sourceRec, Color.White);
                }
            }

            foreach (var p in players) {
                var mouse = Vector2.Transform(that.mouse.LocationV2, cam.Deproject);
                that.batch.Draw(artDebug, p.Location, new Rectangle((currentFrame % framesPerRow) * 32, 0, 32, 32), Color.White, 0f, Vector2.Zero, new Vector2(1f), SpriteEffects.None, 0);
            }
            base.Draw(gameTime);
        }

        public void serverIO() {
            NetOutgoingMessage outMsg = atlasClient.CreateMessage();
            outMsg.Write((byte)AtlasPackets.RequestPositionChange);
            outMsg.Write((short)MoveVector.X);
            outMsg.Write((short)MoveVector.Y);
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
                                UInt16 numPlayers = inMsg.ReadUInt16();
                                for (UInt16 i = 0; i < numPlayers; i++)
                                    players.Add(new DebugPlayer(that, inMsg.ReadInt64(), inMsg.ReadSingle(), inMsg.ReadSingle(), inMsg.ReadString()));
                                break;
                            case AtlasPackets.UpdateMoveVector:
                                numPlayers = inMsg.ReadUInt16();
                                for (UInt16 i = 0; i < numPlayers; i++)
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
            var zoomFactor = 0.1f;
            if (that.kb.IsButtonDown(Keys.Z) || that.kb.IsButtonDown(Keys.PageDown)) {
                cam.Zoom += cam.Zoom < 3 ? zoomFactor : 0;
            } else if (that.kb.IsButtonDown(Keys.X) || that.kb.IsButtonDown(Keys.PageUp)) {
                cam.Zoom -= cam.Zoom > 2 ? zoomFactor : 0;
            } else if (that.kb.IsButtonDown(Keys.C) || that.kb.IsButtonDown(Keys.Home)) {
                cam.Zoom = 1.0f;
            }
        }
    }
}
