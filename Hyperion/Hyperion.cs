using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Configuration;
using Edge.NetCommon;
using Lidgren.Network;
using Edge.Hyperion.UI.Components;
using Edge.Hyperion.UI.Implementation.Screens;
using AssetStore = Edge.Hyperion.Backing.AssetStore;
using Keyboard = Edge.Hyperion.Input.Keyboard;
using Mouse = Edge.Hyperion.Input.Mouse;

namespace Edge.Hyperion {
	public class Hyperion:Game {
		public static void Main(String[] args) {
			using(var game = new Hyperion())
				game.Run();
		}

		GraphicsDeviceManager graphics;
		internal SpriteBatch spriteBatch;
		internal Rectangle bounds;
		//, maestroClient;
		//, maestroConnection;
		internal Keyboard kb;
		internal Mouse mouse;

		public Matrix viewMatrix;

		public Hyperion() {
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
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
			IsMouseVisible = true;
			#endregion
			#region Component Configuration
			spriteBatch = new SpriteBatch(this.GraphicsDevice);
			kb = new Keyboard(this);
			Components.Add(kb);
			mouse = new Mouse(this);
			Components.Add(mouse);
            AssetStore.ButtonTypes.Add(0, new Button.ButtonStyle(Content.Load<Texture2D>(@"../Images/Button/Greybutton.png"), Content.Load<SpriteFont>(@"../Font/Helvetica"), Color.LightGray, Color.Gray, Color.White));
			this.SetScreen(new Splash(this));
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
            
			base.Initialize();
		}

		protected override void LoadContent() {
			base.LoadContent();
		}

		protected override void Update(GameTime gameTime) {
			if(IsActive) {
				bounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
				base.Update(gameTime);
			}
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.White);
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, viewMatrix);
			base.Draw(gameTime);
			spriteBatch.End();
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
