using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using Edge.NetCommon;
using Lidgren.Network;
using Edge.Hyperion.UI.Components;
using Edge.Hyperion.UI.Implementation.Screens;
using AssetStore = Edge.Hyperion.Backing.AssetStore;
using Keyboard = Edge.Hyperion.Input.Keyboard;
using Mouse = Edge.Hyperion.Input.Mouse;

namespace Edge.Hyperion {
    public class Final : Game {
        public static void Main(String[] args) {
            using (var game = new Final())
                game.Run();
        }

        GraphicsDeviceManager graphics;
        internal SpriteBatch batch;
        internal RenderTarget2D target;
        internal Rectangle bounds;
        //, maestroClient;
        //, maestroConnection;
        internal Keyboard kb;
        internal Mouse mouse;
        internal SpriteFont Helvetica;

        internal Matrix viewMatrix;

        internal Color BackColor;

        internal Single tragetScale = 2f;

        public Final() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            #region Event Handlers
            Exiting += OnExit;
            Activated += OnActivated;
            Deactivated += OnDeactivated;
            #endregion
        }

        protected override void Initialize() {
            #region Window
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            #endregion
            #region Component Configuration
            batch = new SpriteBatch(this.GraphicsDevice);
            target = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            GraphicsDevice.SetRenderTarget(target);
            kb = new Keyboard(this);
            Components.Add(kb);
            mouse = new Mouse(this);
            Components.Add(mouse);
            Helvetica = Content.Load<SpriteFont>(@"../Font/Helvetica");
            AssetStore.ButtonTypes.Add(0, new Button.ButtonStyle(Content.Load<Texture2D>(@"../Images/Button/newbtn.jpg"), Helvetica, Color.LightGray, Color.Gray, Color.LightGray));
            AssetStore.PlayerTexture = Content.Load<Texture2D>(@"../Images/Mage.png");
            AssetStore.Pixel = Content.Load<Texture2D>(@"../Images/Grey.png");
            this.SetScreen(new Opening(this, Content.Load<Texture2D>(@"../Images/Logo.png")));
            #endregion
            #region Maestro Configuration
            //var maestroConfig = new NetPeerConfiguration("Maestro");
            //maestroConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            //maestroConfig.Port = 2346;
            //maestroClient = new NetClient(maestroConfig);
            //maestroClient.Start();
            //send out discovery signal for local Maestro server
            //maestroClient.DiscoverLocalPeers(2345);
            #endregion

            BackColor = new Color(60, 88, 111);
            base.Initialize();
        }

        protected override void LoadContent() {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime) {
            if (IsActive) {
                bounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(BackColor);
            batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);
            batch.Draw(target, new Rectangle(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height), Color.White);
            batch.End();

            GraphicsDevice.SetRenderTarget(null);
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, viewMatrix);
            base.Draw(gameTime);
            batch.End();
        }

        void OnExit(object sender, EventArgs e) {
            //TODO: This needs to be passed to individual Screens to notify the appropriate servers of the User prompted disconnect (this won't trigger if it's a crash, or internet issue)

        }

        protected override void OnActivated(object sender, EventArgs e) {
        }

        protected override void OnDeactivated(object sender, EventArgs e) {
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
