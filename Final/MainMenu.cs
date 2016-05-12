using System.Collections.Generic;
using Thread = System.Threading.Thread;
using Edge.Hyperion.UI.Components;

using Microsoft.Xna.Framework.Media;
using Game = Microsoft.Xna.Framework.Game;
using Point = Microsoft.Xna.Framework.Point;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

using Town = Edge.Hyperion.Engine.Town;

using btn = Edge.Hyperion.UI.Components.Button;

using AssetStore = Edge.Hyperion.Backing.AssetStore;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
    public class MainMenu : Screen {
        string title = "Online";
        internal NetClient atlasClient;
        string Port, Address;
        List<btn> btnList = new List<btn>();
        Point mapSize = new Point(AssetStore.TownSize);
        SoundEffectInstance music;
        float timer;
        float opening = 10f;
        string ip = "10.53.5.241";

        List<DebugPlayer> players = new List<DebugPlayer>();
        List<Enemy> enemys = new List<Enemy>();

        public MainMenu(Game game) : base(game)
        {
            var Port = "2348";
            var Address = ip;
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
            music = AssetStore.MainmenuSong.CreateInstance();
            var init = new Point(0, 175);
            var Height = 45;
            var Width = 100;
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 2 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.basic], "Play", () => {
                music.Stop();
                atlasClient.Disconnect("bye");
                atlasClient.Shutdown("bye");
                Thread.Sleep(1000);
                that.SetScreen(new Town(that, ip, "2348"));
            }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 3 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.disabled], "Options", () =>
            { }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 4 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.disabled], "Credits", () =>
            { }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 5 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.basic], "Exit", () => {
                that.Exit();
            }));
            //strip = new MenuStrip(that, this, Vector2.Zero, btnList);
            //that.Components.Add(strip);
            foreach (btn button in btnList) {
                button.DrawOrder = 1;
                that.Components.Add(button);
            }
            music.IsLooped = true;
            music.Volume = 0f;
            music.Play();
            cam.Position.X = AssetStore.Ground.Width / 4;
            base.Initialize();
        }

        protected override void LoadContent() {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            foreach (var btn in btnList) {
                var width = 100;
                btn._location.X = viewport.Width / 2 - width / 2;
            }
            var dt = gameTime.ElapsedGameTime;
            if (timer < opening &&(music.Volume + .5f / (60 * opening)) < 1)
                music.Volume += .5f / (60 * opening);
            cam.Position = Vector2.Lerp(AssetStore.mouse.LocationV2, cam.Position/100, .95f);
            serverIO();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            that.batch.Draw(AssetStore.Ground, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(1.5f), SpriteEffects.None, 0f);

            //strip.Update();
            //Point firstPos = new Point(AssetStore.TileSize);
            //for (int y = 0; y < mapSize.Y; y++) {
            //    for (int x = 0; x < mapSize.X; x++) {
            //        Rectangle rec = new Rectangle((x * AssetStore.TileSize) - firstPos.X, (y * AssetStore.TileSize) - firstPos.Y, AssetStore.TileSize, AssetStore.TileSize);
            //        Rectangle sourceRec = Tile.GetScorceRectangle(int.Parse(Map.Rows[y][x]));
            //        that.batch.Draw(Tile.TileSetTexture, rec, sourceRec, Color.White);
            //    }
            //}
            foreach (var e in enemys)
            { // Main loop to draw each enemy to the world
                //that.batch.Draw(AssetStore.Pixel, e.hitBox, Color.Red);
                that.batch.Draw(e.Type.Base, e.hitBox, new Rectangle((e.currentframe % 2) * e.Width, e.mult * e.Height, e.Width, e.Height), e.Type.BaseColour);
                //that.batch.Draw(AssetStore.Pixel, e.hitBox, new Color(Color.Red, 100));
            }
            foreach (var p in players)
            { // Main loop to draw every player that is connected to the server.world
                var mouse = Vector2.Transform(AssetStore.mouse.LocationV2, cam.Deproject);
                var scale = 0.25f;
                var mesurments = that.Helvetica.MeasureString(p.Name);
                var location = new Vector2((p.X + p.Width / 2) - mesurments.X / 2 * scale, p.Y - p.Width / 2 - 8);
                if (p.isAttacking)
                {
                    p.AttackRec = p.mult == 0 ? new Rectangle(p.HitBox.X + p.HitBox.Width, p.HitBox.Y, p.HitBox.Width, p.HitBox.Height) : p.AttackRec;
                    p.AttackRec = p.mult == 1 ? new Rectangle(p.HitBox.X - p.HitBox.Width, p.HitBox.Y, p.HitBox.Width, p.HitBox.Height) : p.AttackRec;
                    p.AttackRec = p.mult == 2 ? new Rectangle(p.HitBox.X, p.HitBox.Y - p.HitBox.Width, p.HitBox.Width, p.HitBox.Height) : p.AttackRec;
                    p.AttackRec = p.mult == 3 ? new Rectangle(p.HitBox.X, p.HitBox.Y + p.HitBox.Width, p.HitBox.Width, p.HitBox.Height) : p.AttackRec;
                }
                else
                {
                    p.AttackRec = new Rectangle();
                }
                if (p.isAttacking) that.batch.Draw(AssetStore.PlayerTexture, p.HitBox, new Rectangle(3 * p.Width, p.mult * p.Height, p.Width, p.Height), Color.White);
                if (!p.isAttacking) that.batch.Draw(AssetStore.PlayerTexture, p.HitBox, new Rectangle(((p.currentFrame) % 2) * p.Width, p.mult * p.Height, p.Width, p.Height), Color.White);
                that.batch.Draw(AssetStore.PlayerTexture, p.AttackRec, new Rectangle(4 * p.Width, p.mult * p.Height, p.Width, p.Height), Color.White);
                //that.batch.Draw(AssetStore.Pixel, p.AttackRec, new Color(Color.Red, 100));
            }
            base.Draw(gameTime);
            that.batch.End();
            that.batch.Begin();
            that.batch.Draw(AssetStore.Pixel, new Vector2(viewport.Width / 2 - viewport.Width / 6, 0), null, new Color(50, 50, 50, 150), 0f, Vector2.Zero, new Vector2(viewport.Width / 3, viewport.Height), SpriteEffects.None, 0f);
            //for (int i = 0; i < btnList.Count; i++)
            //    btnList[i].Draw(gameTime);
            DrawCenter(title);
            that.batch.End();
        }

        public void serverIO()
        {
            NetIncomingMessage inMsg;
            while ((inMsg = atlasClient.ReadMessage()) != null)
            {
                switch (inMsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        switch ((NetCommon.AtlasPackets)inMsg.ReadByte())
                        {
                            case NetCommon.AtlasPackets.UpdatePositions:
                                players.Clear();
                                ushort numPlayers = inMsg.ReadUInt16();
                                for (ushort i = 0; i < numPlayers; i++)
                                {
                                    long NetID = inMsg.ReadInt64();
                                    int X = inMsg.ReadInt32();
                                    int Y = inMsg.ReadInt32();
                                    string Name = inMsg.ReadString();
                                    float health = inMsg.ReadFloat();
                                    players.Add(new DebugPlayer(NetID, X, Y, Name, health));
                                    players[i].mult = inMsg.ReadInt32();
                                    players[i].currentFrame = inMsg.ReadInt32();
                                    players[i].isAttacking = inMsg.ReadBoolean();
                                    players[i].gold = inMsg.ReadInt32();
                                    players[i].lifeTimer = inMsg.ReadFloat();
                                }
                                break;
                            case NetCommon.AtlasPackets.UpdateEnemy:
                                enemys.Clear();
                                ushort numEnemys = inMsg.ReadUInt16();
                                for (ushort i = 0; i < numEnemys; i++)
                                {
                                    long ID = inMsg.ReadInt64();
                                    int x = inMsg.ReadInt32();
                                    int y = inMsg.ReadInt32();
                                    int type = inMsg.ReadInt32();
                                    int current = inMsg.ReadInt32();
                                    int mult = inMsg.ReadInt32();
                                    enemys.Add(new Enemy(ID, x, y, AssetStore.EnemyTypes[(NetCommon.Type)type]));
                                    enemys[i].currentframe = current;
                                    enemys[i].mult = mult;

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

        public void DrawCenter(string text) {
            var measure = that.Helvetica.MeasureString(text);
            var location = new Vector2(viewport.Width / 2 - measure.X / 2, 50);
            that.batch.DrawString(that.Helvetica, text, location, Color.White);
        }
    }
}
