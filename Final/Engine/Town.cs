using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.Backing;

namespace Edge.Hyperion.Engine {
    public class Town : Screen {

        TileMap Map = new TileMap(@"Map\grassDemo.csv");
        Point mapSize = new Point(5);

        public Town(Game game) : base(game) {
            
        }

        protected override void LoadContent() {
            Tile.TileSetTexture = that.Content.Load<Texture2D>(@"..\Images\Tiles\tileset");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            Vector2 firstSquare = new Vector2(cam.Position.X / AssetStore.TileSize, cam.Position.Y / AssetStore.TileSize);
            Point firstPos = new Point((int)firstSquare.X, (int)firstSquare.Y);

            Vector2 squareOffset = new Vector2(cam.Position.X % AssetStore.TileSize, cam.Position.Y % AssetStore.TileSize);
            Point offset = new Point((int)squareOffset.X, (int)squareOffset.Y);

            for (int y = 0; y < mapSize.Y; y++) {
                for(int x = 0; x < mapSize.X; x++) {
                    Rectangle rec = new Rectangle((x * AssetStore.TileSize) - offset.X, (y * AssetStore.TileSize) - offset.Y, AssetStore.TileSize, AssetStore.TileSize);
                    Rectangle sourceRec = Tile.GetScorceRectangle(Map.Rows[y + firstPos.Y].Columns[x + firstPos.X].TileID);
                    that.batch.Draw(Tile.TileSetTexture, rec, sourceRec, Color.White);
                }
            }
            base.Draw(gameTime);
        }
    }
}
