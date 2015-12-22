﻿using System;
using System.Collections.Generic;
using Edge.Hyperion.UI.Components;
using Edge.Hyperion.UI.Implementation.Popups;
using Edge.Hyperion.UI.Implementation.Popups.Panel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetStore = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion.UI.Implementation.Screens {

	public class MainMenu:Screen {
		//The main menu, handles interactions with maestro
		//For now just a demo for ui componets
        List<Button> btnList = new List<Button>();
        List<Popup> popList = new List<Popup>();
		Texture2D backGround;

		public MainMenu(Game game) : base(game) {
            btnList.Add(new Button(that, this, new Rectangle(), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.basic], "Home", () => {
                that.SetScreen(new Splash(that));
            }));
			btnList.Add(new Button(that, this, new Rectangle(), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.basic], "Play", () => {
                that.SetScreen(new Splash(that));
            }));
            btnList.Add(new Button(that, this, new Rectangle(), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.basic], "Hats", () => {
                that.SetScreen(new Splash(that));
            }));
            btnList.Add(new Button(that, this, new Rectangle(), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.basic], "Quit", () => {
                that.Exit();
            }));
		}

		public override void Initialize() {
            that.Components.Add(new MenuStrip(that, this, Vector2.Zero, btnList));
            base.Initialize();
		}

		protected override void LoadContent() {
			base.LoadContent();
		}

		public override void Update(GameTime gameTime) {
      /*
      //Let's just keep This all in the right place... (might be changing it around later to use FlatBuffs, idk yet)
              if(kb.IsButtonToggledDown(Keys.S)){
                  NetOutgoingMessage start = maestroClient.CreateMessage();
                  start.Write((byte)MaestroPackets.StartLobby);
                  start.Write(-1);
                  maestroClient.SendMessage(start, maestroConnection, NetDeliveryMethod.ReliableUnordered);
              }

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
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
		}
	}
}
