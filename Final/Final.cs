using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.UI.Components;
using AssetStore = Edge.Hyperion.Backing.AssetStore;
using Keyboard = Final.Input.Keyboard;
using Mouse = Final.Input.Mouse;


namespace Edge.Hyperion {
    public class Final : Game {
        public static void Main(String[] args) {
            using (var game = new Final())
                game.Run();
        }

        GraphicsDeviceManager graphics;
        internal SpriteBatch batch;
        internal Keyboard kb;
        internal Mouse mouse;

        internal Matrix viewMatrix;

        internal SpriteFont Helvetica;

        public Final() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        void OnExit(object sender, EventArgs e) {
            //TODO: This needs to be passed to individual Screens to notify the appropriate servers of the User prompted disconnect (this won't trigger if it's a crash, or internet issue)

        }
        protected override void Initialize() {
            #region Window
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            #endregion
            #region Component Configuration
            batch = new SpriteBatch(GraphicsDevice);
            kb = new Keyboard(this);
            mouse = new Mouse(this);
            Components.Add(kb);
            Components.Add(mouse);
            Helvetica = Content.Load<SpriteFont>(@"../Font/Helvetica");
            AssetStore.ButtonTypes.Add(0, new Button.Style(Content.Load<Texture2D>(@"../Images/Button/newbtn.jpg"), Helvetica, Color.LightGray, Color.Gray, Color.LightGray));
            AssetStore.PlayerTexture = Content.Load<Texture2D>(@"../Images/Mage.png");
            AssetStore.Pixel = Content.Load<Texture2D>(@"../Images/Grey.png");
            SetScreen(new MainMenu(this));
            #endregion
            base.Initialize();
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.TransparentBlack);
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, viewMatrix);
            base.Draw(gameTime);
            batch.End();
        }

        internal void SetScreen(Screen newScreen) {
            viewMatrix = default(Matrix);
            Components.Clear();
            Components.Add(kb);
            Components.Add(mouse);
            Components.Add(newScreen);
        }
    }
}
