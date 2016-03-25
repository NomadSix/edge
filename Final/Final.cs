using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.UI.Components;
using AssetStore = Edge.Hyperion.Backing.AssetStore;
using Keyboard = Final.Input.Keyboard;
using Mouse = Final.Input.Mouse;
using xTile.Display;

namespace Edge.Hyperion {
    public class Final : Game {
        public static void Main(string[] args) {
            using (var game = new Final())
                game.Run();
        }

        private Screen _currentScreen;
        private Screen[] _currentScreens;

        public SpriteFont GreySpriteFont;
        public Texture2D GreyImageMap;
        public string GreyMap;

        GraphicsDeviceManager graphics;
        internal SpriteBatch batch;
        internal Keyboard kb;
        internal Mouse mouse;

        internal Matrix viewMatrix;
        internal SamplerState sampleState;

        internal SpriteFont Helvetica;

        public delegate void Action();

        bool contentLoaded = false;
        Thread thread;

        public object NativeMethods { get; private set; }

        public Final() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        void OnExit(object sender, EventArgs e) {
            //TODO: This needs to be passed to individual Screens to notify the appropriate servers of the User prompted disconnect (this won't trigger if it's a crash, or internet issue)
            
        }

        protected override void Initialize() {
            #region Window
            graphics.PreferredBackBufferWidth = AssetStore.Width;
            graphics.PreferredBackBufferHeight = AssetStore.Height;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.ApplyChanges();
            Window.Position = new Point(AssetStore.Width / 4, AssetStore.Height / 4);
            #endregion
            thread = new Thread(new ThreadStart(LoadLostsOfContent));
            thread.Name = "Content Loading Thread";
            thread.Start();

            base.Initialize();
        }

        void LoadLostsOfContent() {
            #region Component Configuration
            batch = new SpriteBatch(GraphicsDevice);
            kb = new Keyboard(this);
            mouse = new Mouse(this);
            Components.Add(kb);
            Components.Add(mouse);
            AssetStore.ButtonTypes.Add(Button.Style.Type.basic, new Button.Style(Content.Load<Texture2D>(@"../Images/Grey.png"), Content.Load<Texture2D>(@"../Images/Button/TitleButton.png"), Helvetica, Color.TransparentBlack, Color.SkyBlue, Color.White));
            AssetStore.ButtonTypes.Add(Button.Style.Type.disabled, new Button.Style(Content.Load<Texture2D>(@"../Images/Grey.png"), Content.Load<Texture2D>(@"../Images/Button/TitleButton.png"), Helvetica, Color.TransparentBlack, Color.TransparentBlack, Color.Gray));
            AssetStore.PlayerTexture = Content.Load<Texture2D>(@"../Images/Sheets/Player/LinkSheet.png");
            AssetStore.Pixel = Content.Load<Texture2D>(@"../Images/Grey.png");
            AssetStore.Mouse = Content.Load<Texture2D>(@"../Images/Mouse/placeholder.png");
            Popup.backGround = AssetStore.Pixel;
            var hud = new UI.Implementation.Hud.StatusBar(this, 1.0f);
            hud.Visible = false;
            Components.Add(hud);
            SetScreen(new MainMenu(this));
            #endregion
            contentLoaded = true;
        }

        protected override void LoadContent() {
            Helvetica = Content.Load<SpriteFont>(@"../Font/Helvetica");
            sampleState = SamplerState.LinearWrap;
            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.TransparentBlack);
            batch.Begin(SpriteSortMode.Deferred, null, sampleState, null, null, null, viewMatrix);
            if (contentLoaded) {
                base.Draw(gameTime);
            } else {
                //draw loading screen
                DrawCenter("Loading ...");
            }
            batch.End();
        }

        void DrawCenter(String text) {
            var measure = Helvetica.MeasureString(text);
            var location = new Vector2(GraphicsDevice.Viewport.Width / 2 - measure.X / 2, 50);
            batch.DrawString(Helvetica, text, location, Color.White);
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
