using System;
using Edge.Hyperion.UI.Components;
using Edge.Hyperion.UI.Implementation.Popups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetStore = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion.UI.Implementation.Screens {

	public class MainMenu:Screen {
		//The main menu, handles interactions with maestro
		//For now just a demo for ui componets
		Button btnPlay;
		Button btnAlert;
		Texture2D backGround;

		public MainMenu(Game game) : base(game) {
			btnPlay = new Button(that, this, new Rectangle(0, 0, 100, 50), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.test], "Play", () => {
                that.SetScreen(new Splash(that));
            });
            btnAlert = new Button(that, this, new Rectangle(125, 0, 100, 50), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.test], "Alert", () => {
                this._isActive = false;
                that.Components.Add(new Alert(that, this, new Vector2(100, 100), 300, 300));
            });
		}

		public override void Initialize() {
			that.Components.Add(btnPlay);
            that.Components.Add(btnAlert);
            that.viewMatrix = cam.ViewMatrix;
			base.Initialize();
		}

		protected override void LoadContent() {
			//backGround = that.Content.Load<Texture2D>(@"../");
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
			//that.spriteBatch.Draw(backGround, that.bounds, Color.White);
			base.Draw(gameTime);
		}
	}
}
