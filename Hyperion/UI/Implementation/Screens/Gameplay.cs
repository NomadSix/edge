﻿using System;
using System.Linq;
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
        Vector2 MoveVector;
	    Boolean isLeft;
        Rectangle player;

		List<DebugPlayer> players = new List<DebugPlayer>();

        Int32 currentFrame = 0;
        Int32 totalFrames = 0;
        Int32 framesPerRow = 0;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 200;

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
			base.Initialize();
		}

		protected override void LoadContent() {
			pixel = new Texture2D(GraphicsDevice, 1, 1);
			pixel.SetData<Color>(new[] { Color.White });
			artDebug = that.Content.Load<Texture2D>(@"..\Images\Sheets\Player\MageWalkingSprite.png");
		    totalFrames = artDebug.Width/32;
		    framesPerRow = totalFrames;
			backGround = that.Content.Load<Texture2D>(@"..\Images\Basic_Background.png");
            base.LoadContent();
		}
         
		public override void Update(GameTime gameTime) {
            if (that.kb.IsButtonDown(Keys.Z) || that.kb.IsButtonDown(Keys.PageDown)) {
				cam.Zoom -= .1f;
			} else if (that.kb.IsButtonDown(Keys.X) || that.kb.IsButtonDown(Keys.PageUp)) {
				cam.Zoom += .1f;
			} else if (that.kb.IsButtonDown(Keys.C) || that.kb.IsButtonDown(Keys.Home)) {
				cam.Zoom = 1f;
			}

            if (that.kb.IsButtonDown(Keys.A) || that.kb.IsButtonDown(Keys.Left)) {
		        MoveVector.X = -1;
            } else if (that.kb.IsButtonDown(Keys.D) || that.kb.IsButtonDown(Keys.Right)) {
		        MoveVector.X = 1;
		    } else {
		        MoveVector.X = 0;
		    }

            if (that.kb.IsButtonDown(Keys.W) || that.kb.IsButtonDown(Keys.Up)) {
		        MoveVector.Y = -1;
            } else if (that.kb.IsButtonDown(Keys.S) | that.kb.IsButtonDown(Keys.Down)) {
		        MoveVector.Y = 1;
		    } else {
		        MoveVector.Y = 0;
		    }

		    if (MoveVector != Vector2.Zero) {
		        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
		        if (timeSinceLastFrame > millisecondsPerFrame) {
		            timeSinceLastFrame -= millisecondsPerFrame;
		            currentFrame += 1;
		        }
		    }


			NetOutgoingMessage outMsg = atlasClient.CreateMessage();
            outMsg.Write((byte)AtlasPackets.RequestPositionChange);
            outMsg.Write((Int16)MoveVector.X);
            outMsg.Write((Int16)MoveVector.Y);
            outMsg.Write(Environment.UserName);
			atlasClient.SendMessage(outMsg, NetDeliveryMethod.ReliableSequenced);
			
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
									players.Add(new DebugPlayer(inMsg.ReadInt64(), inMsg.ReadSingle(), inMsg.ReadSingle(), inMsg.ReadString()));
                                break;
                            case AtlasPackets.UpdateMoveVector:								
                                numPlayers = inMsg.ReadUInt16();
						        for (UInt16 i = 0; i < numPlayers; i++)
						            players[i].MoveVector = new Vector2(inMsg.ReadSingle(), inMsg.ReadSingle());
                                break;
						}
						break;
					case NetIncomingMessageType.DiscoveryResponse:
						atlasClient.Connect(inMsg.SenderEndPoint);
						break;
				}

                foreach (var ent in players.Where(x => x.NetID == atlasClient.UniqueIdentifier)) {
                    Vector2 position = new Vector2(ent.Location.X - that.GraphicsDevice.Viewport.Width / 2, ent.Location.Y - that.GraphicsDevice.Viewport.Height / 2);
                    //position = Vector2.Lerp(position, Vector2.Zero, .5f);
                    cam.Position = position;
                }
				base.Update(gameTime);
			}
		}

        public override void Draw(GameTime gameTime) {
            that.batch.Draw(backGround, Vector2.Zero, null, null, null, 0f, new Vector2(.45f), Color.White, SpriteEffects.None, 0);
			foreach(var p in players) {
			    if (p.MoveVector.X < 0) {
			        isLeft = true;
			    }
			    else if (p.MoveVector.X > 0){
			        isLeft = false;
			    }
				//Color n = new Color((int)Math.Abs(p.NetID % 255), (int)Math.Abs(p.NetID % 254), (int)Math.Abs(p.NetID % 253), 255);
                that.batch.DrawString(that.Helvetica, p.Name, new Vector2(p.Location.X - (that.Helvetica.MeasureString(Environment.UserName).X *.5f) / 4f, p.Location.Y - that.Helvetica.MeasureString(Environment.UserName).Y*.5f), Color.Black, 0f, Vector2.Zero, new Vector2(.5f), SpriteEffects.None, 0f);
                that.batch.Draw(artDebug, p.Location, new Rectangle((currentFrame % framesPerRow) * 32, 0, 32, 32), Color.White, 0f, Vector2.Zero, new Vector2(2f), isLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			}
			base.Draw(gameTime);
		}
	}
}
