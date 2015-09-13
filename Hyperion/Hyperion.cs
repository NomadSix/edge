using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Edge.NetCommon;
using Lidgren.Network;
using Keyboard = Edge.Hyperion.Input.Keyboard;

namespace Edge.Hyperion {
	public class Hyperion: Game {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		NetClient maestroClient, atlasClient;
		NetConnection maestroConnection, atlasConnection;

		Keyboard kb;

		Texture2D pixel;

		public Hyperion() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize() {
			base.Initialize();

			#region Window Configuration
			graphics.PreferMultiSampling = true;
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			#endregion

			#region Component Configuration
			spriteBatch = new SpriteBatch(GraphicsDevice);
			kb = new Keyboard(this);
			Components.Add(kb);
			#endregion

			#region Maestro Configuration
			var maestroConfig = new NetPeerConfiguration("Maestro");
			maestroConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
			maestroConfig.Port = 2346;
			maestroClient = new NetClient(maestroConfig);
			maestroClient.Start();
			//send out discovery signal for local Maestro server
			maestroClient.DiscoverLocalPeers(2345);
			#endregion
			#region Atlas Configuration
			var atlasConfig = new NetPeerConfiguration("Maestro");
			atlasConfig.Port = 2347;
			atlasClient = new NetClient(atlasConfig);
			atlasClient.Start();
			#endregion
		}

		protected override void LoadContent() {
			pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData<Color>(new[]{ Color.White });
		}

		protected override void Update(GameTime gameTime) {
			if(IsActive) {
				if(kb.IsButtonToggledDown(Keys.Escape))
					Exit(); //Replace this...
				
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

				base.Update(gameTime);
			}
		}

		protected override void Draw(GameTime gameTime) {
			graphics.GraphicsDevice.Clear(Color.Black);
			base.Draw(gameTime);
		}
	}
}