using System;
using Lidgren.Network;
using Edge.NetCommon;
using Microsoft.Xna.Framework;

namespace Edge.Atlas {
	public partial class Atlas {
		void NetworkIncomingLoop() {
			while(!isExiting) {
				NetIncomingMessage msg;
				while((msg = server.ReadMessage()) != null) {
					switch(msg.MessageType) {
						case NetIncomingMessageType.Data:
							switch((AtlasPackets)msg.ReadByte()) {
								case AtlasPackets.RequestPositionChange:
									players[msg.SenderConnection.RemoteUniqueIdentifier].MovingTo = new Vector2(msg.ReadUInt16(), msg.ReadUInt16());
									break;
							}
							break;
						case NetIncomingMessageType.StatusChanged:
							switch(msg.SenderConnection.Status) {
								case NetConnectionStatus.Connected:
									players.Add(msg.SenderConnection.RemoteUniqueIdentifier, new DebugPlayer());
									break;
								case NetConnectionStatus.Disconnected:
									players.Remove(msg.SenderConnection.RemoteUniqueIdentifier);
									break;
							}
							break;
						case NetIncomingMessageType.DebugMessage:
						case NetIncomingMessageType.VerboseDebugMessage:
						case NetIncomingMessageType.WarningMessage:
						case NetIncomingMessageType.ErrorMessage:
							Console.WriteLine(msg.ReadString());
							break;
					}
				}
			}
		}
	}
}

