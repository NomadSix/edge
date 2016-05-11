using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Net;
using Edge.Maestro.Net;
using Edge.Maestro.Lobbies;
using Edge.NetCommon;
using System.Threading;

namespace Edge.Maestro {
	public class Maestro {

		public Boolean isExiting;
		public readonly List<ClientInstance> instances = new List<ClientInstance>();
		public readonly List<Lobby> lobbies = new List<Lobby>();

		public readonly Dictionary<Int32, Atlas.Atlas> serverPool = new Dictionary<Int32, Atlas.Atlas>();

		NetServer server;

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args) {new Maestro().Run();}

		/// <summary>
		/// Initializes a new instance of the <see cref="Edge.Maestro.Maestro"/> class.
		/// </summary>
		public Maestro() {
			var config = new NetPeerConfiguration("Maestro");
			config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
			config.Port = 2345;
			server = new NetServer(config);
			server.Start();
		}

		/// <summary>
		/// Start the main server loop
		/// </summary>
		public void Run() {
			var inputThread = new Thread(InputHandler);
			inputThread.Start(); 

			while(!isExiting) {
				#region NetIncoming
				NetIncomingMessage msg;
				while((msg = server.ReadMessage()) != null) {
					switch(msg.MessageType) {
						case NetIncomingMessageType.Data:
							switch((MaestroPackets)msg.ReadByte()) {
								case MaestroPackets.CreateLobby:
									//make a new lobby
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
									//Accept or declines an invite to a lobby
									var lobbyParticipant = lobbies[msg.ReadInt32()].Members.Find(x => x.UName == Lookup(msg).UName);
									lobbyParticipant.IsResponded = true;
									lobbyParticipant.IsConnected = msg.ReadBoolean();
									break;
								case MaestroPackets.StartLobby:
									//Starts a game with the players in a lobby
									var lobbyUID = msg.ReadInt32();
									if(lobbyUID == -1) {
										//inQueue.Add(Lookup(msg).UName);
										break;
									}
									var lobby = lobbies.Find(x => x.LobbyUID == lobbyUID);
									if(lobby.Host == Lookup(msg).UName) {
										var portNum = GetNextAvailablePort();
										var newServer = new Atlas.Atlas(portNum, true);
										serverPool.Add(portNum, newServer);
										var introduction = server.CreateMessage();
										introduction.Write((byte)MaestroPackets.IntroduceAtlas);
										//introduction.Write({IPAddress});
										introduction.Write(portNum);
										lobby.Members.ForEach(x => server.SendMessage(introduction, Lookup(x.UName).Connection, NetDeliveryMethod.ReliableUnordered));
									}
									break;
							}
							break;
						case NetIncomingMessageType.StatusChanged:
							switch(msg.SenderConnection.Status) {
								case NetConnectionStatus.Connected:
									//On new connection, add a client instance to keep track of it
									instances.Add(new ClientInstance(msg.SenderConnection, msg.SenderConnection.RemoteHailMessage.ReadString()));
									break;
								case NetConnectionStatus.Disconnected:
									//On disconnect, we no longer need the client instance
									//TODO: Check if this is working
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
							//Print all debug/diagnostics/warning messages to the console
							Console.WriteLine(msg.ReadString());
							break;
					}
				}
				#endregion

				//TODO: Queue, instance management, data storage

				//TODO: Timing
			}

			server.Shutdown("shutting down");
		}

		/// <summary>
		/// Handles input from the console.
		/// </summary>
		void InputHandler() {
			while(!isExiting) {
				string readLine = Console.ReadLine();
				#region Parse out the command and arguments
				if(readLine == null)
					return;
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
					case "CONTROL":
						if(!serverPool.ContainsKey(Int32.Parse(args[0]))) {
							Console.WriteLine("Server not found on port " + args[0]);
							break;
						}
						var instance = serverPool[Int32.Parse(args[0])];
						instance.Control(args[1], args.Skip(2).ToList());
						break;
					case "LIST_ATLAS":
						Console.WriteLine("Currently Used Ports");
						foreach(var a in serverPool)
							Console.WriteLine(a.Key);
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
		}

		/// <summary>
		/// Gets the next available port.
		/// </summary>
		/// <returns>The next available port.</returns>
		Int32 GetNextAvailablePort(){
			Int32 port = 2348;
			while(true) {
				if(!serverPool.ContainsKey(port))
					return port;
				port++;
			}
		}

		/// <summary>
		/// Finds the ClientInstance associated with a given message
		/// </summary>
		/// <param name="msg">A message sent by the client</param>
		ClientInstance Lookup(NetIncomingMessage msg) {
			var n = instances.Find(x => x.UUID == msg.SenderConnection.RemoteUniqueIdentifier); 
			return n;
		}

		/// <summary>
		/// Finds the ClientInstance controlling a username
		/// </summary>
		/// <param name="uname">The client's Username</param>
		ClientInstance Lookup(String uname) {
			var n = instances.Find(x => x.UName == uname);
			return n;
		}
	}
}

