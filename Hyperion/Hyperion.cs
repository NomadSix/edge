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
			IsMouseVisible = true;
			#endregion
			#region Component Configuration
			spriteBatch = new SpriteBatch(this.GraphicsDevice);
			kb = new Keyboard(this);
			Components.Add(kb);
			mouse = new Mouse(this);
			Components.Add(mouse);
			AssetStore.ButtonTypes.Add(0, new Button.ButtonStyle(Content.Load<Texture2D>(@"../Images/base.png"), Color.LightGray, Color.Gray, null));
			this.SetScreen(new MainMenu(this));
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

				//Replace this...
				//to get rid of the warning as much as anything, but should keep a general eye on this
                
				/*
                #region Extract to lobby/queue screen
                if(kb.IsButtonToggledDown(Keys.S)){
                    NetOutgoingMessage start = maestroClient.CreateMessage();
                    start.Write((byte)MaestroPackets.StartLobby);
                    start.Write(-1);
                    maestroClient.SendMessage(start, maestroConnection, NetDeliveryMethod.ReliableUnordered);
                }
                #endregion

                NetIncomingMessage msg;
                while((msg = maestroClient.ReadMessage()) != null) {
                    switch(msg.MessageType) {
                        case NetIncomingMessageType.Data:
                            switch((MaestroPackets)msg.ReadByte()) {
                                case MaestroPackets.InviteToLobby:
                                    break;
                                case MaestroPackets.LobbyStatus:
                                    break;
                                case MaestroPackets.IntroduceAtlas:
                                    atlasClient.Connect(msg.ReadString(), msg.ReadInt32());
                                    break;
                            }
                            break;
                        case NetIncomingMessageType.DiscoveryResponse:
                            NetOutgoingMessage hail = maestroClient.CreateMessage();
                            const string uname = "Test User 0";
                            hail.Write(uname);
                            maestroClient.Connect(msg.SenderEndPoint, hail);
                            break;
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            System.Diagnostics.Debug.WriteLine(msg.ReadString());
                            break;
                    }
                }
                */
				bounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
				base.Update(gameTime);
			}
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.White);
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
			base.Draw(gameTime);
			spriteBatch.End();
		}

		void OnExit(object sender, EventArgs e) {
			//atlasClient.Disconnect("Client Closing");
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
