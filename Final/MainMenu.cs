using System.Collections.Generic;
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

using TileMap = Edge.Hyperion.Engine.TileMap;
using Tile = Edge.Hyperion.Engine.Tile;
using Town = Edge.Hyperion.Engine.Town;

using btn = Edge.Hyperion.UI.Components.Button;

using AssetStore = Edge.Hyperion.Backing.AssetStore;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
    public class MainMenu : Screen {
        string title = "Online";
        List<btn> btnList = new List<btn>();
        TileMap Map = new TileMap(@"Map\grassDemo.csv");
        Point mapSize = new Point(AssetStore.TownSize);
        SoundEffectInstance music;
        float timer;
        float opening = 10f;

        public MainMenu(Game game) : base(game) { }

        public override void Initialize() {
            music = AssetStore.MainmenuSong.CreateInstance();
            var init = new Point(0, 175);
            var Height = 45;
            var Width = 100;
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 2 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.basic], "Play", () => {
                music.Volume = 1f;
                music.Volume = music.Volume / 4;
                that.SetScreen(new Town(that, "127.0.0.1", "2348"));
            }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 3 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.disabled], "Options", () =>
            {

            }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 4 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.disabled], "Credits", () =>
            {

            }));
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
            base.Initialize();
        }

        protected override void LoadContent() {
            Tile.TileSetTexture = that.Content.Load<Texture2D>(@"..\Images\Sheets\Tiles\GrassSheet");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            var dt = gameTime.ElapsedGameTime;
            if (timer < opening &&(music.Volume + .5f / (60 * opening)) < 1)
                music.Volume += .5f / (60 * opening);
            cam.Position = Vector2.Lerp(AssetStore.mouse.LocationV2, cam.Position/4, .9f);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            //strip.Update();
            Point firstPos = new Point(AssetStore.TileSize * 20);
            for (int y = 0; y < mapSize.Y; y++) {
                for (int x = 0; x < mapSize.X; x++) {
                    Rectangle rec = new Rectangle((x * AssetStore.TileSize) - firstPos.X, (y * AssetStore.TileSize) - firstPos.Y, AssetStore.TileSize, AssetStore.TileSize);
                    Rectangle sourceRec = Tile.GetScorceRectangle(int.Parse(Map.Rows[y][x]));
                    that.batch.Draw(Tile.TileSetTexture, rec, sourceRec, Color.White);
                }
            }
            that.batch.End();
            that.batch.Begin();
            base.Draw(gameTime);
            that.batch.Draw(AssetStore.Pixel, new Vector2(viewport.Width / 2 - viewport.Width / 6, 0), null, new Color(50, 50, 50, 150), 0f, Vector2.Zero, new Vector2(viewport.Width / 3, viewport.Height), SpriteEffects.None, 0f);
            //for (int i = 0; i < btnList.Count; i++)
            //    btnList[i].Draw(gameTime);
            DrawCenter(title);
            that.batch.End();
        }

        public void DrawCenter(string text) {
            var measure = that.Helvetica.MeasureString(text);
            var location = new Vector2(viewport.Width / 2 - measure.X / 2, 50);
            that.batch.DrawString(that.Helvetica, text, location, Color.White);
        }
    }
}
