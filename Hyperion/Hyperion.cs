using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Configuration;
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
		internal SpriteBatch spriteBatch;
		NetClient atlasClient;
		//, maestroClient;
		NetConnection atlasConnection;
		//, maestroConnection;
		List<DebugPlayer> players = new List<DebugPlayer>();
		internal Keyboard kb;
		internal Mouse mouse;
		Camera2D cam;

		Texture2D pixel;
		Texture2D backGround;
		Texture2D artDebug;

		Vector2 moveVector;

        Int64 NetID;

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
			atlasConnection = atlasClient.Connect(ConfigurationManager.AppSettings["DebugAtlasAddress"], 2348);
			#endregion

			LoadContent();
		}

		protected override void LoadContent() {
			pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData<Color>(new[]{ Color.White });
			artDebug = Content.Load<Texture2D>(@"..\Images\Eagle.png");
			backGround = Content.Load<Texture2D>(@"..\Images\Basic_Background.png");
			cam = new Camera2D(new Vector2(0, 0));
		}

		protected override void Update(GameTime gameTime) {
			if(IsActive) {

				if(kb.IsButtonDown(Keys.Z)) {
					cam.Zoom -= .1f;
				}
				else if(kb.IsButtonDown(Keys.X)) {
					cam.Zoom += .1f;
				}

				if(kb.IsButtonToggledDown(Keys.Escape))
					Exit(); //Replace this...
				//to get rid of the warning as much as anything, but should keep a general eye on this
				Console.WriteLine(atlasConnection.AverageRoundtripTime);
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
					outMsg.Write((UInt16)moveVector.X);
					outMsg.Write((UInt16)moveVector.Y);
					atlasClient.SendMessage(outMsg, NetDeliveryMethod.ReliableSequenced);
				}
				//move vector stuff here
			}
			NetIncomingMessage inMsg;
			while((inMsg = atlasClient.ReadMessage()) != null) {
				switch(inMsg.MessageType) {
					case NetIncomingMessageType.Data:
                        switch ((AtlasPackets)inMsg.ReadByte())
                        {
                            //Would this work? We need a way for the clients to know there own id's
                            //case AtlasPackets.FirstID:
                            //    NetID = inMsg.ReadInt64();
                            //    break;
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
			//cam.Position = players[0].Location;
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, cam.ViewMatrix);
			foreach(var p in players) {
				//Color n = new Color((int)Math.Abs(p.NetID % 255), (int)Math.Abs(p.NetID % 254), (int)Math.Abs(p.NetID % 253), 255);
				spriteBatch.Draw(artDebug, p.Location, null, null, null, 0f, new Vector2(1, 1), Color.White, SpriteEffects.None, 0);
			}
			spriteBatch.Draw(backGround, Vector2.Zero, null, null, null, 0f, new Vector2(.25f), Color.White, SpriteEffects.None, 0);
			spriteBatch.End();
			base.Draw(gameTime);
		}

		void OnExit(object sender, EventArgs e) {
			//atlasClient.Disconnect("Client Closing");
		}

		protected override void OnActivated(object sender, EventArgs e) {
		}

		protected override void OnDeactivated(object sender, EventArgs e) {
		}
	}
}
