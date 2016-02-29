using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Edge.Hyperion.Engine;
using btn = Edge.Hyperion.UI.Components.Button;
using MenuStrip = Edge.Hyperion.UI.Implementation.Popups.Panel.MenuStrip;
using AssetStore = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion {
    public class MainMenu : Screen {
        String title = "<title>";
        MenuStrip strip;
        List<btn> btnList = new List<btn>();

        public MainMenu(Game game) : base(game) { }

        public override void Initialize() {
            var init = new Point(0, 175);
            var Height = 45;
            var Width = 100;
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.basic], "Play", () => {
                that.SetScreen(new Town(that));
            }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 2 * Height, 0, 0), AssetStore.ButtonTypes[btn.Style.Type.basic], "", () => { }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 3 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.basic], "Options", () => {

            }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 4 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.basic], "Credits", () => {

            }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 5 * Height, 0, 0), AssetStore.ButtonTypes[btn.Style.Type.basic], "", () => { }));
            btnList.Add(new btn(that, this, new Rectangle(viewport.Width / 2 - Width / 2, init.Y + 6 * Height, Width, Height), AssetStore.ButtonTypes[btn.Style.Type.basic], "Exit", () => {
                that.Exit();
            }));
            //strip = new MenuStrip(that, this, Vector2.Zero, btnList);
            //that.Components.Add(strip);
            foreach (var btn in btnList)
                that.Components.Add(btn);
            base.Initialize();
        }

        public override void Draw(GameTime gameTime) {
            //strip.Update();
            DrawCenter(title);
            base.Draw(gameTime);
        }

        public void DrawCenter(String text) {
            var measure = that.Helvetica.MeasureString(text);
            var location = new Vector2(viewport.Width / 2 - measure.X / 2, 50);
            that.batch.DrawString(that.Helvetica, text, location, Color.White);
        }
    }
}
