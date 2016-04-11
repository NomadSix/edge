using Screen2 = System.Windows.Forms.Screen;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using ThreadStart = System.Threading.ThreadStart;
using Thread = System.Threading.Thread;

using AssetStore = Edge.Hyperion.Backing.AssetStore;
using Edge.Hyperion.Engine;
using Edge.Hyperion.UI.Components;

namespace Edge.Hyperion {
    public class Final : Game {
        public static void Main(string[] args) {
            using (var game = new Final())
                game.Run();
        }

        public SpriteFont GreySpriteFont;
        public Texture2D GreyImageMap;
        public string GreyMap;

        GraphicsDeviceManager graphics;
        internal SpriteBatch batch;

        internal Matrix viewMatrix;
        internal SamplerState sampleState;

        internal SpriteFont Helvetica;

        public delegate void Action();

        bool contentLoaded = false;
        Thread ContentThread;

        public object NativeMethods { get; private set; }

        public Final() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            #region Window
            graphics.PreferredBackBufferWidth = AssetStore.Width;
            graphics.PreferredBackBufferHeight = AssetStore.Height;
            var Screen = Screen2.PrimaryScreen.WorkingArea.Size;
            Window.Position = new Point((Screen.Width - AssetStore.Width) / 2, (Screen.Height - AssetStore.Height) / 2);
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.ApplyChanges();
            #endregion
        }

        void OnExit(object sender, System.EventArgs e) {
            //TODO: This needs to be passed to individual Screens to notify the appropriate servers of the User prompted disconnect (this won't trigger if it's a crash, or internet issue)
            
        }

        protected override void Initialize() {
            ContentThread = new Thread(new ThreadStart(LoadLostsOfContent));
            ContentThread.Name = "Content Loading Thread";
            ContentThread.Start();

            base.Initialize();
        }

        void LoadLostsOfContent() {
            #region Component Configuration
            batch = new SpriteBatch(GraphicsDevice);
            AssetStore.kb = new Backing.Keyboard(this);
            AssetStore.mouse = new Backing.Mouse(this);
            Components.Add(AssetStore.kb);
            Components.Add(AssetStore.mouse);
            AssetStore.Pixel = Content.Load<Texture2D>(@"../Images/Grey.png");
            AssetStore.Sword = Content.Load<Texture2D>(@"../Images/sword.png");
            AssetStore.PlayerTexture = Content.Load<Texture2D>(@"../Images/Sheets/Player/LinkSheet.png");
            AssetStore.ButtonTypes.Add(Button.Style.Type.basic, new Button.Style(Content.Load<Texture2D>(@"../Images/Grey.png"), Content.Load<Texture2D>(@"../Images/Button/TitleButton.png"), Helvetica, Color.TransparentBlack, Color.SkyBlue, Color.White));
            AssetStore.ButtonTypes.Add(Button.Style.Type.disabled, new Button.Style(Content.Load<Texture2D>(@"../Images/Grey.png"), Content.Load<Texture2D>(@"../Images/Button/TitleButton.png"), Helvetica, Color.TransparentBlack, Color.TransparentBlack, Color.Gray));
            AssetStore.EnemyTypes.Add(Enemy.Style.Type.Minion, new Enemy.Style(Enemy.Style.Type.Minion, Content.Load<Texture2D>(@"../Images/Sheets/MageMinnion.png"), null, null));
            AssetStore.EnemyTypes.Add(Enemy.Style.Type.Mage, new Enemy.Style(Enemy.Style.Type.Mage, Content.Load<Texture2D>(@"../Images/Sheets/MageWalkingSprite"), null, null));
            AssetStore.EnemyTypes.Add(Enemy.Style.Type.Slime, new Enemy.Style(Enemy.Style.Type.Slime, AssetStore.Pixel, null, null));
            AssetStore.EnemyTypes.Add(Enemy.Style.Type.FireMage, new Enemy.Style(Enemy.Style.Type.FireMage, Content.Load<Texture2D>(@"../Images/Sheets/MageMinnion.png"), null, null));
            AssetStore.EnemyTypes.Add(Enemy.Style.Type.Debug, new Enemy.Style(Enemy.Style.Type.Debug, AssetStore.Pixel, null, null));
            AssetStore.ItemTypes.Add(Item.Style.Type.Health, new Item.Style(Content.Load<Texture2D>(@"../Images/Items/HealthPot.png")));
            AssetStore.MainmenuSong = Content.Load<SoundEffect>(@"../Audio/song");
            Popup.backGround = AssetStore.Pixel;
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

        void DrawCenter(string text) {
            var measure = Helvetica.MeasureString(text);
            var location = new Vector2(GraphicsDevice.Viewport.Width / 2 - measure.X / 2, 50);
            batch.DrawString(Helvetica, text, location, Color.White);
        }

        internal void SetScreen(Screen newScreen) {
            viewMatrix = default(Matrix);
            Components.Clear();
            Components.Add(AssetStore.kb);
            Components.Add(AssetStore.mouse);
            Components.Add(newScreen);
        }
    }
}
