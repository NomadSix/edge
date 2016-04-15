using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.UI.Components;
using Edge.Hyperion.Backing;
using Screen2 = System.Windows.Forms.Screen;

namespace Edge.Hyperion {
    public class Functions : Screen {
        public Functions(Game game) : base(game) { }

        public override void Update(GameTime gameTime) {
            if (AssetStore.kb.IsButtonToggledDown(Microsoft.Xna.Framework.Input.Keys.F11))
                setFullScreen();
        }

        public void setFullScreen() {
            IGraphicsDeviceService graphicsService = (IGraphicsDeviceService)Game.Services.GetService(typeof(IGraphicsDeviceService));
            if (graphicsService != null && graphicsService is GraphicsDeviceManager) {
                var graphics = (graphicsService as GraphicsDeviceManager);
                graphics.IsFullScreen = !graphics.IsFullScreen;
                if (graphics.IsFullScreen) {
                    graphics.PreferredBackBufferWidth = AssetStore.Width;
                    graphics.PreferredBackBufferHeight = AssetStore.Height;
                } else {
                    var Screen = Screen2.PrimaryScreen.WorkingArea.Size;
                    graphics.PreferredBackBufferWidth = Screen.Width;
                    graphics.PreferredBackBufferHeight = Screen.Height;
                }
                graphics.ApplyChanges();
            }
        }
    }
}
