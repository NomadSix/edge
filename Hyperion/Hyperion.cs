using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Edge.NetCommon;
using Lidgren.Network;
using Keyboard = Edge.Hyperion.Input.Keyboard;
using Mouse = Edge.Hyperion.Input.Mouse;
using MouseButtons = Edge.Hyperion.Input.MouseButtons;

namespace Edge.Hyperion {
	public class Hyperion:Game {

		public static void Main(String[] args) {
			using(var game = new Hyperion())
				game.Run();
		}

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		NetClient maestroClient, atlasClient;
		NetConnection maestroConnection, atlasConnection;
		List<DebugPlayer> players = new List<DebugPlayer>();
		Keyboard kb;
		Mouse mouse;

		Texture2D pixel;

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
			IsMouseVisible=true;
			#endregion
			#region Component Configuration
			spriteBatch = new SpriteBatch(this.GraphicsDevice);
			kb = new Keyboard(this);
			Components.Add(kb);
			mouse = new Mouse(this);
			Components.Add(mouse);
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
			#region Atlas Configuration
			var atlasConfig = new NetPeerConfiguration("Atlas");
			//maestroConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
			atlasConfig.Port = 2347;
			atlasClient = new NetClient(atlasConfig);
			atlasClient.Start();
			atlasClient.Connect(System.IO.File.ReadAllText("ip.cfg"), 2348);
			#endregion

			LoadContent();
		}

		protected override void LoadContent() {
			pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData<Color>(new[]{ Color.White });
		}

		protected override void Update(GameTime gameTime) {
			if(IsActive) {

				if(kb.IsButtonToggledDown(Keys.Escape))
					Exit(); //Replace this...
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
				if(mouse.IsButtonDown(MouseButtons.Right)) {
					NetOutgoingMessage outMsg = atlasClient.CreateMessage();
					outMsg.Write((byte)AtlasPackets.RequestPositionChange);
					outMsg.Write((UInt16)mouse.Location.X);
					outMsg.Write((UInt16)mouse.Location.Y);
					atlasClient.SendMessage(outMsg, NetDeliveryMethod.ReliableSequenced);
				}
			}
				NetIncomingMessage inMsg;
				while((inMsg = atlasClient.ReadMessage()) != null) {
					switch(inMsg.MessageType) {
						case NetIncomingMessageType.Data:
							switch((AtlasPackets)inMsg.ReadByte()) {
								case AtlasPackets.UpdatePositions:
									players.Clear();

									UInt16 numPlayers = inMsg.ReadUInt16();
									for(UInt16 i = 0; i < numPlayers; i++)
										players.Add(new DebugPlayer(inMsg.ReadInt64(), inMsg.ReadSingle(), inMsg.ReadSingle()));
									break;
							}
							break;
						case NetIncomingMessageType.DiscoveryResponse:
							atlasClient.Connect(inMsg.SenderEndPoint);
							break;
					}
				}

				base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin();
			foreach (var p in players) {
                Color n = new Color(newColor(),1f);
                if (mouse.IsButtonToggledDown(MouseButtons.Left) && p.Location == mouse.LocationV2)
                    n = newColor();
                //else
				    //n = new Color((int)Math.Abs(p.NetID % 255), (int)Math.Abs(p.NetID % 254), (int)Math.Abs(p.NetID % 253),255);
				spriteBatch.Draw(pixel, p.Location, null, null, null, 0, new Vector2(50, 50), n, SpriteEffects.None, 0);
			}
			spriteBatch.End();
			base.Draw(gameTime);
		}

        public Color newColor()
        {
            return new Color(NetRandom.Instance.Next(0, 255), NetRandom.Instance.Next(0, 255), NetRandom.Instance.Next(0,255));
        }

		void OnExit(object sender, EventArgs e) {
			//atlasClient.Disconnect("Client Closing");
		}

		void OnActivated(object sender, EventArgs e) {
		}

		void OnDeactivated(object sender, EventArgs e) {
		}
	}
}