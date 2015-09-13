using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lidgren.Network;
using System.Linq;
using Edge.NetCommon;
using Microsoft.Xna.Framework;

namespace Edge.Atlas {
	public partial class Atlas {
		NetServer server;
		public Boolean isExiting;
		Boolean runningHeadless;
		Dictionary<Int64, DebugPlayer> players = new Dictionary<Int64, DebugPlayer>();

		Int64 lastTime;
		Int64 currentTime = DateTime.UtcNow.Ticks;

		public Atlas(Int32 port, Boolean runningHeadless) {
			this.runningHeadless = runningHeadless;

			var config = new NetPeerConfiguration("Atlas");
			config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
			config.Port = port;
			server = new NetServer(config);
			server.Start();
		}

		public void Run() {
			if(runningHeadless)
				Task.Run(InputHandler);
			
			while(!isExiting) {
				lastTime = currentTime;
				currentTime = DateTime.UtcNow.Ticks;

				#region Incoming messages
				NetIncomingMessage inMsg;
				while((inMsg = server.ReadMessage()) != null) {
					switch(inMsg.MessageType) {
						case NetIncomingMessageType.Data:
							switch((AtlasPackets)inMsg.ReadByte()) {
								case AtlasPackets.RequestPositionChange:
									players[inMsg.SenderConnection.RemoteUniqueIdentifier].MovingTo = new Vector2(inMsg.ReadUInt16(), inMsg.ReadUInt16());
									break;
							}
							break;
						case NetIncomingMessageType.StatusChanged:
							switch(inMsg.SenderConnection.Status) {
								case NetConnectionStatus.Connected:
									players.Add(inMsg.SenderConnection.RemoteUniqueIdentifier, new DebugPlayer(inMsg.SenderConnection.RemoteUniqueIdentifier));
									break;
								case NetConnectionStatus.Disconnected:
									players.Remove(inMsg.SenderConnection.RemoteUniqueIdentifier);
									break;
							}
							break;
						case NetIncomingMessageType.DiscoveryRequest:
							//Automatic discovery response
							NetOutgoingMessage response = server.CreateMessage();
							server.SendDiscoveryResponse(response, inMsg.SenderEndPoint);
							break;
						case NetIncomingMessageType.DebugMessage:
						case NetIncomingMessageType.VerboseDebugMessage:
						case NetIncomingMessageType.WarningMessage:
						case NetIncomingMessageType.ErrorMessage:
							Console.WriteLine(inMsg.ReadString());
							break;
					}
				}
				#endregion

				Parallel.ForEach(players.Values, PlayerUpdate);

				#region Outgoing Updates
				NetOutgoingMessage outMsg = server.CreateMessage();
				outMsg.Write((byte)AtlasPackets.UpdatePositions);
				outMsg.Write((UInt16)players.Count);
				foreach(var p in players.Values) {
					outMsg.Write(p.NetID);
					outMsg.Write((UInt16)p.Position.X);
					outMsg.Write((UInt16)p.Position.Y);
				}
				server.SendToAll(outMsg, NetDeliveryMethod.UnreliableSequenced);
				#endregion
			}

			server.Shutdown("Bye!");
		}

		void InputHandler() {
			while(!isExiting) {
				string readLine = Console.ReadLine();
				#region Parse out the command and arguments
				if(String.IsNullOrWhiteSpace(readLine)) return;
				string line = readLine.ToUpper();

				string command;
				try {
					command = line.Substring(0, line.IndexOf('('));
				}
				catch(Exception) {
					command = line;
				}

				var args = new List<String>();
				try {
					args = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - (line.IndexOf('(') + 1)).Split(',').ToList();
				}
				catch(Exception) {
					try {
						String argStr = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - (line.IndexOf('(') + 1));
						if(!String.IsNullOrWhiteSpace(argStr))
							args.Add(argStr);
					}
					// Analysis disable once EmptyGeneralCatchClause
					catch {
					}
				}
				#endregion

				try {
					Control(command, args);
				}
				catch(NotSupportedException e) {
					Console.WriteLine(e.Message);
				}
			}
		}

		public void Control(String command, List<String> args) {
			switch(command) {
				case "EXIT":
					isExiting = true;
					break;
				case "CLEAR":
				case "CLS":
					Console.Clear();
					break;
				default:
					String argList = String.Empty;
					args.ForEach(arg => argList += arg + ",");
					throw new NotSupportedException(String.Format("Unrecognised command\nCommand: {0}\nArgs: {1}", command, argList));
			}
		}
	}
}

