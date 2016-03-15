using System;
using System.Collections.Generic;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.Engine;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using AssetStore = Edge.Hyperion.Backing.AssetStore;
using Edge.Hyperion.UI.Components;

namespace Edge.Hyperion.UI.Implementation.Popups {
    public class PauseMenu : Popup {

        List<Button> ButtonList = new List<Button>();
        Screen World;
        Vector2 init;
        public PauseMenu(Game game, Screen world) : base(game) {
            World = world;
        }

        public override void Initialize() {
            _isActive = false;
            that.sampleState = SamplerState.PointWrap;
            init = Vector2.Zero;
            int Height = 45;
            int Width = 100;
            ButtonList.Add(new Button(that, this, new Rectangle(viewport.Width / 2 - Width / 2, (int)init.Y + 6 * Height, Width, Height), AssetStore.ButtonTypes[Button.Style.Type.basic], "Resume", () => {
                _isActive = false;
                that.sampleState = SamplerState.PointClamp;
                foreach (var btn in ButtonList)
                    that.Components.Remove(btn);
                Kill();
            })); 
            ButtonList.Add(new Button(that, this, new Rectangle(viewport.Width / 2 - Width / 2, (int)init.Y + 7 * Height, Width, Height), AssetStore.ButtonTypes[Button.Style.Type.disabled], "Options", () =>
            {
                that.sampleState = SamplerState.LinearWrap;
                World._isActive = false;
                that.SetScreen(new MainMenu(that));
            }));
            ButtonList.Add(new Button(that, this, new Rectangle(viewport.Width / 2 - Width / 2, (int)init.Y + 8 * Height, Width, Height), AssetStore.ButtonTypes[Button.Style.Type.basic], "Exit", () => {
                that.sampleState = SamplerState.LinearWrap;
                World._isActive = false;
                that.SetScreen(new MainMenu(that));
            }));
            foreach (Button button in ButtonList)
                that.Components.Add(button);
            base.Initialize();
        }

        protected override void LoadContent() {
            base.LoadContent();
        }

        public void update(Vector2 pos, Camera2D cam2) {
            if (that.kb.IsButtonToggledDown(Keys.Escape)) {
                that.sampleState = SamplerState.PointClamp;
                _isActive = !_isActive;
                Kill();
            }
            for (int i = 0; i < ButtonList.Count; i++)
                ButtonList[i].update(Vector2.Transform(new Vector2(pos.X, pos.Y + (int)init.Y + i * 45), cam.ViewMatrix), cam2);
        }

        public void draw(Vector2 pos) {
            that.batch.End();
            that.batch.Begin();
            if (_isActive) {
                that.batch.Draw(backGround, Vector2.Zero, null, new Color(50, 50, 50, 75), 0f, Vector2.Zero, new Vector2(viewport.Width, viewport.Height), SpriteEffects.None, 0f);
                that.batch.Draw(backGround, new Vector2(viewport.Width / 2 - viewport.Width / 6, 0), null, new Color(50, 50, 50, 150), 0f, Vector2.Zero, new Vector2(viewport.Width / 3, viewport.Height), SpriteEffects.None, 0f);
                for (int i = 0; i < ButtonList.Count; i++)
                    ButtonList[i].draw(Vector2.Transform(new Vector2(pos.X, pos.Y + (int)init.Y + i * 45), cam.ViewMatrix));
                DrawCenter("Paused", pos);
            }
            that.batch.End();
        }

        public void DrawCenter(string text, Vector2 pos) {
            var measure = that.Helvetica.MeasureString(text);
            var location = Vector2.Transform(new Vector2(viewport.Width / 2 - measure.X / 2, 50), cam.ViewMatrix);
            that.batch.DrawString(that.Helvetica, text, location, Color.White);
        }
    }
}
