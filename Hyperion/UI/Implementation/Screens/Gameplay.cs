﻿using System;
using System.Linq;
using Edge.Hyperion.UI.Components;
using System.Collections.Generic;
using Edge.Hyperion.Backing;
using Edge.Hyperion.UI.Effects.Parallax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Edge.NetCommon;
using Lidgren.Network;

namespace Edge.Hyperion.UI.Implementation.Screens {
	public class Gameplay:Screen {
		Texture2D pixel, artDebug;
		NetClient atlasClient;
		String Port, Address;
        Vector2 MoveVector;
	    Boolean isLeft;
        Rectangle playerArm;

		List<DebugPlayer> players = new List<DebugPlayer>();
        List<DebugTower> towers = new List<DebugTower>();
        List<Layer> Backgrounds = new List<Layer>();
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
            //that.Components.Add(new Effects.Parallax(that, background));
            base.LoadContent();
		}
         
		public override void Update(GameTime gameTime) {
            var zoomFactor = 1f;
            if (that.kb.IsButtonDown(Keys.Z) || that.kb.IsButtonDown(Keys.PageDown)) {
				cam.IncreaseZoom(zoomFactor);
			} else if (that.kb.IsButtonDown(Keys.X) || that.kb.IsButtonDown(Keys.PageUp)) {
                cam.IncreaseZoom(-zoomFactor);
			} else if (that.kb.IsButtonDown(Keys.C) || that.kb.IsButtonDown(Keys.Home)) {
                cam.IncreaseZoom(-2);
			}

            if (that.kb.IsButtonDown(Keys.A) || that.kb.IsButtonDown(Keys.Left)) {
		        MoveVector.X = that.kb.IsButtonDown(Keys.LeftShift) || that.kb.IsButtonDown(Keys.RightShift) ? -2 : -1;
            } else if (that.kb.IsButtonDown(Keys.D) || that.kb.IsButtonDown(Keys.Right)) {
                MoveVector.X = that.kb.IsButtonDown(Keys.LeftShift) || that.kb.IsButtonDown(Keys.RightShift) ? 2 : 1;
		    } else {
		        MoveVector.X = 0;
		    }

            if (that.kb.IsButtonToggledDown(Keys.W) || that.kb.IsButtonToggledDown(Keys.Up)) {
		        MoveVector.Y = -1;
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

            foreach (var player in players.Where(x => x.NetID == atlasClient.UniqueIdentifier)) {
                cam.Position = -player.Location;
            }
		}

        public override void Draw(GameTime gameTime) {
            
            #region background
            //that.batch.Draw(backGround, Vector2.Zero, null, null, null, 0f, new Vector2(.45f), Color.White, SpriteEffects.None, 0);
            foreach (var background in Backgrounds) {
                background.Draw(that.batch);
            }
            #endregion
            foreach (var p in players) {
                var mouse = Vector2.Transform(that.mouse.LocationV2, cam.Deproject);
                if (mouse.X < p.Location.X + 16) {
			        isLeft = true;
			    }
                else {
			        isLeft = false;
			    }
                playerArm = new Rectangle(p.Location.ToPoint(), new Point(32));
			    playerArm.X += playerArm.Width/2;
			    playerArm.Y += playerArm.Height/2;
				//Color n = new Color((int)Math.Abs(p.NetID % 255), (int)Math.Abs(p.NetID % 254), (int)Math.Abs(p.NetID % 253), 255);
                //that.batch.DrawString(that.Helvetica, p.Name, new Vector2(p.Location.X - (that.Helvetica.MeasureString(Environment.UserName).X *.5f) / 4f, p.Location.Y - that.Helvetica.MeasureString(Environment.UserName).Y*.5f-20), Color.Black, 0f, Vector2.Zero, new Vector2(.5f), SpriteEffects.None, 0f);
                that.batch.Draw(artDebug, p.Location, new Rectangle((currentFrame % framesPerRow) * 32, 0, 32, 32), Color.White, 0f, Vector2.Zero, new Vector2(1f), isLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                that.batch.Draw(AssetStore.Pixel, new Rectangle((int)p.Location.X, (int)p.Location.Y-20, (int)(artDebug.Width*p.Health), 20), Color.Green);
                }
            foreach (var player in players.Where(x => x.NetID == atlasClient.UniqueIdentifier)) {
                //MousePosition = new Vector2((Mouse.GetState().X - DrawTransform.Translation.X) * (1 / Gamecode.Camera1.Scale), (Mouse.GetState().Y - DrawTransform.Translation.Y) * (1 / Gamecode.Camera1.Scale));
                var mouse = Vector2.Transform(that.mouse.LocationV2, cam.Deproject);
                var dirrection = player.Location - mouse;
                var art = that.Content.Load<Texture2D>(@"..\Images\MageArms.png");
                that.batch.Draw(art, playerArm, null, Color.White, (float)((Math.Atan2(dirrection.Y, dirrection.X)) + 2 * Math.PI), new Vector2(art.Width / 2, art.Height / 2), isLeft ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                Console.WriteLine(string.Format("{0} + \n + {1} + \n + {2}", mouse, dirrection, player.Location));
            }
            foreach (var tower in towers) {
                var scale = 5f;
                var text = AssetStore.TowerStyles[DebugTower.TowerStyle.Team.Good].BaseTexture;
                that.batch.Draw(text, tower.Location, null, tower.Team == DebugTower.TowerStyle.Team.Good ? Color.White : Color.Red, 0f, Vector2.Zero, scale, tower.Location.X > 1000 ? SpriteEffects.None: SpriteEffects.FlipHorizontally, 0f);
                that.batch.Draw(AssetStore.Pixel, new Rectangle((int)tower.Location.X, (int)tower.Location.Y - 20, (int)(scale *text.Width * tower.Health), 20), Color.Green);
            }
            base.Draw(gameTime);
        }
    }
}
