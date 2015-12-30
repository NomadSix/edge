using System;
using Edge.Hyperion.UI.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Buffers = Edge.NetCommon.Atlas;
using Lidgren.Network;
using FlatBuffers;

using Mouse = Edge.Hyperion.Input.Mouse;

namespace Edge.Hyperion.UI.Implementation.Screens {
	public class Gameplay:Screen {

		Texture2D pixel, backGround, artDebug;
		NetClient atlasClient;
		String Port, Address;

		List<DebugPlayer> entities = new List<DebugPlayer>();

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
			//move vector stuff here
			NetIncomingMessage inMsg;
			while((inMsg = atlasClient.ReadMessage()) != null) {
				switch(inMsg.MessageType) {
					case NetIncomingMessageType.Data:
						var buff = new ByteBuffer(inMsg.ReadBytes(inMsg.LengthBytes));
						var packet = Buffers.Packet.GetRootAsPacket(buff);
						switch(packet.DataType) {
							case Buffers.PacketData.EntityPulse:
								var pulse = new Buffers.EntityPulse();
								packet.GetData<Buffers.EntityPulse>(pulse);
								for(int i = 0; i < pulse.EntitiesLength; i++) {
									NetCommon.Atlas.Entity temp = pulse.GetEntities(i);
									entities.Remove(entities.Find(x => x.NetID == temp.Id)); //TODO: WE GOTTA FIND SOMETHING BETTER THAN LINQ ITS SO SLOW. ALSO DELTAS AND STUFF
									//TODO: Figure out how we really want to keep track of entities, then add them to the List from here
								}
								break;
							case Buffers.PacketData.StatusEvent:
								var statusEvent = new Buffers.StatusEvent();
								packet.GetData<Buffers.StatusEvent>(statusEvent);
								switch(statusEvent.Id) {
									case Buffers.Events.PlayerDisconnect:
										//Notify user of teammate disconnecting or whatever
										break;
									case Buffers.Events.PlayerReconnect:
										//Notify user or something
										break;
								}
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
			that.spriteBatch.Draw(backGround, Vector2.Zero, null, null, null, 0f, new Vector2(.25f), Color.White, SpriteEffects.None, 0);
			foreach(var p in entities) {
				Color n = new Color((int)Math.Abs(p.NetID % 255), (int)Math.Abs(p.NetID % 254), (int)Math.Abs(p.NetID % 253), 255);
				that.spriteBatch.Draw(artDebug, p.Location, null, null, null, 0f, new Vector2(1, 1), n, SpriteEffects.None, 0);
			}
			base.Draw(gameTime);
		}
	}
}
