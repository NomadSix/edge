using System;
using Edge.Hyperion.UI.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Edge.NetCommon;
using Lidgren.Network;

using Mouse = Edge.Hyperion.Input.Mouse;

namespace Edge.Hyperion.UI.Implementation.Screens {
	public class Gameplay:Screen {

		Texture2D pixel, backGround, artDebug;
		NetClient atlasClient;
		String Port, Address;

		List<DebugPlayer> players = new List<DebugPlayer>();

		public Gameplay(Game game, String address, String port) : base(game) {
			Address = address;
			Port = port;
		}

		public override void Initialize() {
			#region Atlas Configuration
			var atlasConfig = new NetPeerConfiguration("Atlas");
			//maestroConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
			atlasConfig.Port = 2347;
			atlasClient = new NetClient(atlasConfig);
			atlasClient.Start();
			atlasClient.Connect(Address, int.Parse(Port));
			#endregion
			LoadContent();
			base.Initialize();
		}

		protected override void LoadContent() {
			pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData<Color>(new[] { Color.White });
			artDebug = that.Content.Load<Texture2D>(@"..\Images\Eagle.png");
			backGround = that.Content.Load<Texture2D>(@"..\Images\Basic_Background.png");
		}

		public override void Update(GameTime gameTime) {
			if(that.kb.IsButtonDown(Keys.Z)) {
				cam.Zoom -= .1f;
			}
			else if(that.kb.IsButtonDown(Keys.X)) {
				cam.Zoom += .1f;
			}
			else if(that.kb.IsButtonDown(Keys.C)) {
				cam.Zoom = 1f;
			}
            that.viewMatrix = cam.ViewMatrix;

			if(that.mouse.IsButtonDown(Mouse.MouseButtons.Right)) {
				NetOutgoingMessage outMsg = atlasClient.CreateMessage();
				outMsg.Write((byte)AtlasPackets.RequestPositionChange);
				var mouseNormal = Vector2.Transform(that.mouse.LocationV2, cam.Deproject);
				outMsg.Write((UInt16)mouseNormal.X);
				outMsg.Write((UInt16)mouseNormal.Y);
				atlasClient.SendMessage(outMsg, NetDeliveryMethod.ReliableSequenced);
			}
			//move vector stuff here
			NetIncomingMessage inMsg;
			while((inMsg = atlasClient.ReadMessage()) != null) {
				switch(inMsg.MessageType) {
					case NetIncomingMessageType.Data:
						switch((AtlasPackets)inMsg.ReadByte()) {
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
				//cam.Position = players[0].Location;
				base.Update(gameTime);
			}
		}

		public override void Draw(GameTime gameTime) {
			foreach(var p in players) {
				that.spriteBatch.Draw(backGround, Vector2.Zero, null, null, null, 0f, new Vector2(.25f), Color.White, SpriteEffects.None, 0);
				Color n = new Color((int)Math.Abs(p.NetID % 255), (int)Math.Abs(p.NetID % 254), (int)Math.Abs(p.NetID % 253), 255);
				that.spriteBatch.Draw(artDebug, p.Location, null, null, null, 0f, new Vector2(1, 1), n, SpriteEffects.None, 0);
			}
			base.Draw(gameTime);
		}
	}
}
