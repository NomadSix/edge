using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Edge.Hyperion.Engine;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.UI.Implementation.Popups;
using btn = Edge.Hyperion.UI.Components.Button;
using AssetStore = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion {
    public class MainMenu : Screen {
        string title = "B.U.T.T.S. Online";
        List<btn> btnList = new List<btn>();
        TileMap Map = new TileMap(@"Map\grassDemo.csv");
        Point mapSize = new Point(AssetStore.TownSize);

        public MainMenu(Game game) : base(game) { }

        public override void Initialize() {
            var init = new Point(0, 175);
            var Height = 45;
            var Width = 100;
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2 + 16, init.Y + 2 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.basic], "Play", () => {
                that.SetScreen(new Town(that, "127.0.0.1", "2348"));
            }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2 + 16, init.Y + 3 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.disabled], "Options", () =>
            {

            }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2 + 16, init.Y + 4 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.disabled], "Credits", () =>
            {

            }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2 + 16, init.Y + 5 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.basic], "Exit", () => {
                that.Exit();
            }));
            //strip = new MenuStrip(that, this, Vector2.Zero, btnList);
            //that.Components.Add(strip);
            foreach (btn button in btnList)
                that.Components.Add(button);

            base.Initialize();
        }

        protected override void LoadContent() {
            Tile.TileSetTexture = that.Content.Load<Texture2D>(@"..\Images\Sheets\Tiles\GrassSheet");
            base.LoadContent();
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
            that.batch.Draw(AssetStore.Pixel, new Vector2(viewport.Width / 2 - viewport.Width / 6, 0), null, new Color(50, 50, 50, 150), 0f, Vector2.Zero, new Vector2(viewport.Width / 3, viewport.Height), SpriteEffects.None, 0f);
            for (int i = 0; i < btnList.Count; i++)
                btnList[i].Draw(gameTime);
            DrawCenter(title);
            DrawMouse();
            base.Draw(gameTime);
        }

        public void DrawCenter(String text) {
            var measure = that.Helvetica.MeasureString(text);
            var location = new Vector2(viewport.Width / 2 - measure.X / 2, 50);
            that.batch.DrawString(that.Helvetica, text, location, Color.White);
        }
    }
}
