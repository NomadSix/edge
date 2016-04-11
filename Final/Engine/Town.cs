using System.Collections.Generic;
using System.Linq;
using Lidgren.Network;
using Edge.Hyperion.UI.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
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

        TileMap Map = new TileMap(@"Map\grassDemo.csv");
        Point mapSize = new Point(AssetStore.TownSize);
        Vector2 MoveVector;

        MouseState old, newer;

        PauseMenu pMenu;
        StatusBar statusBar;

        List<DebugPlayer> players = new List<DebugPlayer>();
        List<Enemy> enemys = new List<Enemy>();
        List<Item> items = new List<Item>();
        
        int framesPerRow = 2;

        public Town(Game game, string address, string port) : base(game) {
            Port = port;
            Address = address;
            cam.Zoom = 2f;
            #region Atlas Configuration
            var Config = new NetPeerConfiguration("Atlas");
            Config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            Config.Port = 2347;
            atlasClient = new NetClient(Config);
            atlasClient.Start();
            ////maestroConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            atlasClient.Connect(Address, int.Parse(Port));
            #endregion
        }

        public override void Initialize() {
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

            if (AssetStore.kb.IsButtonToggledDown(Keys.F11))
                setFullScreen();

            serverIO();
            foreach (var player in players.Where(x => x.NetID == atlasClient.UniqueIdentifier)) {
                if (pMenu._isActive == false) {
                    cameraControl();
                    playerMovment(gameTime, player);
                }
                PlayerIO(player);
                cam.Position = new Vector2(player.X - viewport.Width / 2, player.Y - viewport.Height / 2);
            }

            if (pMenu._isActive) {
                MoveVector = Vector2.Zero;
            }
            pMenu.update(new Vector2(cam.Position.X - 50f + viewport.Width / 2.0f + 16, cam.Position.Y - 50f + viewport.Height / 2.0f + 16), cam);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            that.GraphicsDevice.Clear(new Color(79, 154, 73));
            Point firstPos = new Point(mapSize.X / 2 * AssetStore.TileSize);
            for (int y = 0; y < mapSize.Y; y++) { // Main loop to draw the tile map .csv to the world
                for (int x = 0; x < mapSize.X; x++) {
                    Rectangle rec = new Rectangle((x * AssetStore.TileSize) - firstPos.X, (y * AssetStore.TileSize) - firstPos.Y, AssetStore.TileSize, AssetStore.TileSize);
                    Rectangle sourceRec = Tile.GetScorceRectangle(int.Parse(Map.Rows[y][x]));
                    that.batch.Draw(Tile.TileSetTexture, rec, sourceRec, Color.White);
                }
            }

            foreach (var i in items) {
                that.batch.Draw(i.type.Base, i.Hitbox, Color.White);
            }
            foreach (var e in enemys) { // Main loop to draw each enemy to the world
                //that.batch.Draw(AssetStore.Pixel, e.hitBox, Color.Red);
                that.batch.Draw(e.Type.Base, e.hitBox, new Rectangle((e.currentframe % framesPerRow) * e.Width, e.mult * e.Width, e.Width, e.Height), e.Type.BaseColour);
                that.batch.Draw(AssetStore.Pixel, e.hitBox, new Color(Color.Red, 100));
            }
            foreach (var p in players) { // Main loop to draw every player that is connected to the server
                var mouse = Vector2.Transform(AssetStore.mouse.LocationV2, cam.Deproject);
                var scale = 0.25f;
                var mesurments = that.Helvetica.MeasureString(p.Name);
                var location = new Vector2((p.X + p.Width / 2) - mesurments.X / 2 * scale, p.Y - p.Width / 2);
                that.batch.DrawString(that.Helvetica, pMenu._isActive ? string.Empty : p.Name, location, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                that.batch.Draw(artDebug, p.HitBox, new Rectangle((p.currentFrame % framesPerRow) * p.Width, p.mult * p.Width, p.Width, p.Width), Color.White);
                that.batch.Draw(AssetStore.Pixel, p.HitBox, new Color(Color.Red, 100));

                if (p.isAttacking) {
                    p.AttackRec = p.mult == 0 ? new Rectangle(p.HitBox.X - p.HitBox.Width, p.HitBox.Y, p.HitBox.Width, p.HitBox.Height) : p.AttackRec;
                    p.AttackRec = p.mult == 1 ? new Rectangle(p.HitBox.X, p.HitBox.Y + p.HitBox.Width, p.HitBox.Width, p.HitBox.Height) : p.AttackRec;
                    p.AttackRec = p.mult == 2 ? new Rectangle(p.HitBox.X, p.HitBox.Y - p.HitBox.Width, p.HitBox.Width, p.HitBox.Height) : p.AttackRec;
                    p.AttackRec = p.mult == 3 ? new Rectangle(p.HitBox.X + p.HitBox.Width, p.HitBox.Y, p.HitBox.Width, p.HitBox.Height) : p.AttackRec;
                } else {
                    p.AttackRec = new Rectangle();
                }
                that.batch.Draw(AssetStore.Pixel, p.AttackRec, new Color(Color.Red, 100));

            }
            foreach (var p in players.Where(x => x.NetID == atlasClient.UniqueIdentifier)) {
                statusBar.draw(p.Health);
            }
            pMenu.draw(new Vector2(cam.Position.X - 50f + viewport.Width / 2.0f + 16, cam.Position.Y - 50f + viewport.Height / 2.0f + 16));
        }

        public void serverIO() {
            NetIncomingMessage inMsg;
            while ((inMsg = atlasClient.ReadMessage()) != null) {
                switch (inMsg.MessageType) {
                    case NetIncomingMessageType.Data:
                        switch ((AtlasPackets)inMsg.ReadByte()) {
                            case AtlasPackets.UpdatePositions:
                                players.Clear();
                                ushort numPlayers = inMsg.ReadUInt16();
                                for (ushort i = 0; i < numPlayers; i++) {
                                    long NetID = inMsg.ReadInt64();
                                    int X = inMsg.ReadInt32();
                                    int Y = inMsg.ReadInt32();
                                    string Name = inMsg.ReadString();
                                    float health = inMsg.ReadFloat();
                                    players.Add(new DebugPlayer(NetID, X, Y, Name, health));
                                    players[i].mult = inMsg.ReadInt32();
                                    players[i].currentFrame = inMsg.ReadInt32();
                                    players[i].isAttacking = inMsg.ReadBoolean();
                                }
                                break;
                            case AtlasPackets.UpdateEnemy:
                                enemys.Clear();
                                ushort numEnemys = inMsg.ReadUInt16();
                                for (ushort i = 0; i < numEnemys; i++) {
                                    enemys.Add(new Enemy(inMsg.ReadInt64(), inMsg.ReadInt32(), inMsg.ReadInt32(), AssetStore.EnemyTypes[(Enemy.Style.Type)inMsg.ReadInt32()]));
                                    enemys[i].currentframe = inMsg.ReadInt32();
                                    enemys[i].mult = inMsg.ReadInt32();
                                }
                                break;
                            case AtlasPackets.UpdateItem:
                                items.Clear();
                                ushort numItems = inMsg.ReadUInt16();
                                for (ushort i = 0; i < numItems; i++) {
                                    items.Add(new Item(inMsg.ReadInt64(), inMsg.ReadInt32(), inMsg.ReadInt32(), AssetStore.ItemTypes[(Item.Style.Type)inMsg.ReadInt32()]));
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

        public void PlayerIO(DebugPlayer p) {
            NetOutgoingMessage outMsg = atlasClient.CreateMessage();
            outMsg.Write((byte)AtlasPackets.RequestPositionChange);
            outMsg.Write((short)p.MoveVector.X);
            outMsg.Write((short)p.MoveVector.Y);
            outMsg.Write(System.Environment.UserName);
            outMsg.Write(p.isAttacking);
            atlasClient.SendMessage(outMsg, NetDeliveryMethod.ReliableSequenced);
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
                p.MoveVector.Y = -1;
            } else if (AssetStore.kb.IsButtonDown(Keys.S) || AssetStore.kb.IsButtonDown(Keys.Down)) {
                p.MoveVector.Y = 1;
            } else {
                p.MoveVector.Y = 0;
            }

            if (AssetStore.kb.IsButtonDown(Keys.A) || AssetStore.kb.IsButtonDown(Keys.Left)) {
                p.MoveVector.X = -1;
            } else if (AssetStore.kb.IsButtonDown(Keys.D) || AssetStore.kb.IsButtonDown(Keys.Right)) {
                p.MoveVector.X = 1;
            } else {
                p.MoveVector.X = 0;
            }

            if (AssetStore.kb.IsButtonToggledDown(Keys.Space) || AssetStore.mouse.IsButtonToggledDown(Backing.Mouse.MouseButtons.Left)) {
                p.isAttacking = true;
            } else {
                p.isAttacking = false;
            }
        }

        public void setFullScreen() {
            IGraphicsDeviceService graphicsService = (IGraphicsDeviceService)Game.Services.GetService(typeof(IGraphicsDeviceService));
            if (graphicsService != null && graphicsService is GraphicsDeviceManager) {
                (graphicsService as GraphicsDeviceManager).IsFullScreen = true;
                (graphicsService as GraphicsDeviceManager).ApplyChanges();
            }
        }
    }
}
