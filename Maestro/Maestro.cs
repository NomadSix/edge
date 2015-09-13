using System;
using System.Linq;
using System.Collections.Generic;
using Lidgren.Network;
using System.Threading.Tasks;
using Edge.Maestro.Net;
using Edge.Maestro.Lobbies;
using Edge.NetCommon;

namespace Edge.Maestro {
	public class Maestro {

		/// <summary>
		/// Notes: 
		/// If we're having issues with interators, it's probably the combination of Lists, ForEaches, and async operations
		/// 
		/// TODO: 
		/// Timing/Delays
		/// </summary>

		Boolean isExiting;
		readonly List<ClientInstance> instances = new List<ClientInstance>();
		readonly List<Lobby> lobbies = new List<Lobby>();
		readonly List<String> inQueue = new List<String>();

		readonly List<Atlas.Atlas> serverPool = new List<Atlas.Atlas>();

		NetServer server;

		public Maestro() {
			var config = new NetPeerConfiguration("Maestro");
			config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
			config.Port = 2345;
			server = new NetServer(config);
			server.Start();
		}

		public void Run() {
			Task.Run(NetworkIncomingLoop);
			Task.Run(LogicLoop);

			while(!isExiting) {
				string readLine = Console.ReadLine();
				#region Parse out the command and arguments
				if(readLine == null) return;
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
				switch(command) {
					case "EXIT":
						isExiting = true;
						break;
					case "CLEAR":
					case "CLS":
						Console.Clear();
						break;
					default:
						Console.Write("Unrecognised command\nCommand: {0}\nArgs: ", command);
						args.ForEach(arg => Console.Write(arg + ","));
						Console.WriteLine();
						break;
				}
			}
			server.Shutdown("shutting down");
		}

		void NetworkIncomingLoop() {
			while(!isExiting) {
				NetIncomingMessage msg;
				while((msg = server.ReadMessage()) != null) {
					switch(msg.MessageType) {
						case NetIncomingMessageType.Data:
							switch((MaestroPackets)msg.ReadByte()) {
								case MaestroPackets.CreateLobby:
									byte playersPerTeam = msg.ReadByte();
									byte numInvites = msg.ReadByte();
									String hostName = Lookup(msg).UName;
								#region Get Next Lobby ID
									Int32 lobbyID = -1;
									lobbies.ForEach(x => lobbyID = x.LobbyUID > lobbyID ? x.LobbyUID : lobbyID);
									lobbyID++;
								#endregion
									lobbies.Add(new Lobby(lobbyID, playersPerTeam, hostName));
									for(byte i = 0; i < numInvites; i++) {
										String sendTo = msg.ReadString();
										NetOutgoingMessage invite = server.CreateMessage();
										invite.Write((Int16)MaestroPackets.InviteToLobby);
										invite.Write(lobbyID);
										invite.Write(hostName);
										server.SendMessage(invite, Lookup(sendTo).Connection, NetDeliveryMethod.ReliableUnordered);
										lobbies[lobbyID].Members.Add(new Participant(sendTo));
									}
									break;
								case MaestroPackets.ReplyToLobbyInvite:
									var lobbyParticipant = lobbies[msg.ReadInt32()].Members.Find(x => x.UName == Lookup(msg).UName);
									lobbyParticipant.IsResponded = true;
									lobbyParticipant.IsConnected = msg.ReadBoolean();
									break;
								case MaestroPackets.StartLobby:
									var lobbyUID = msg.ReadInt32();
									if(lobbyUID == -1) {
										inQueue.Add(Lookup(msg).UName);
										break;
									}
									var lobby = lobbies.Find(x => x.LobbyUID == lobbyUID);
									if(lobby.Host == Lookup(msg).UName) {
										lobby.Members.ForEach(x => inQueue.Add(x.UName));
										lobbies.Remove(lobby);
									}
									break;
							}
							break;
						case NetIncomingMessageType.StatusChanged:
							switch(msg.SenderConnection.Status) {
								case NetConnectionStatus.Connected:
									instances.Add(new ClientInstance(msg.SenderConnection, msg.SenderConnection.RemoteHailMessage.ReadString()));
									break;
								case NetConnectionStatus.Disconnected:
									instances.Remove(Lookup(msg));
									break;
							}
							break;
						case NetIncomingMessageType.DiscoveryRequest:
							//Automatic discovery response
							NetOutgoingMessage response = server.CreateMessage();
							server.SendDiscoveryResponse(response, msg.SenderEndPoint);
							break;
						case NetIncomingMessageType.DebugMessage:
						case NetIncomingMessageType.VerboseDebugMessage:
						case NetIncomingMessageType.WarningMessage:
						case NetIncomingMessageType.ErrorMessage:
						// print diagnostics message
							Console.WriteLine(msg.ReadString());
							break;
					}
				}
			}
		}

		void LogicLoop() {
			while(!isExiting) {
				if(inQueue.Count >= 1) {
					var portNum = 4631;
					var newServer = new Atlas.Atlas(portNum, true);
					serverPool.Add(newServer);
					inQueue.ForEach(x => {
						var introduction = server.CreateMessage();
						introduction.Write((byte)MaestroPackets.InviteToLobby);
						introduction.Write("192.168.20.203");
						introduction.Write(portNum);
						server.SendMessage(introduction, Lookup(x).Connection, NetDeliveryMethod.ReliableUnordered);
					});
				}
				//TODO: Queue things, running Atlas instances
				//TODO: Data storage after Atlas finishes [sqlite?]
			}
		}

		ClientInstance Lookup(NetIncomingMessage msg) {
			var n = instances.Find(x => x.UUID == msg.SenderConnection.RemoteUniqueIdentifier); 
			return n;
		}

		ClientInstance Lookup(String uname) {
			var n = instances.Find(x => x.UName == uname);
			return n;
		}
	}
}

