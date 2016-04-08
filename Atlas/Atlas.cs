using System;
using System.Threading;
using System.Collections.Generic;
using Lidgren.Network;
using System.Linq;
using Edge.NetCommon;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace Edge.Atlas {
	public partial class Atlas {
		NetServer server;
        bool runningHeadless;
        public bool isExiting;
		public Dictionary<long, DebugPlayer> players = new Dictionary<long, DebugPlayer>();
		public List<ServerEnemy> enemys = new List<ServerEnemy>();

        public List<ServerEnemy> addEnemys = new List<ServerEnemy>();
        public List<int> removeEnemys = new List<int>();

        public long lastTime;
        public long lastUpdates;
        public long currentTime = DateTime.UtcNow.Ticks;


		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args) {
			if(args.Length > 0)
				try {
					new Atlas(int.Parse(args[0]), true).Run();
				}
				catch (Exception) {
					new Atlas(2348, true).Run();
				}
			else
				new Atlas(2348, true).Run();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Edge.Atlas.Atlas"/> class.
		/// </summary>
		/// <param name="port">Port.</param>
		/// <param name="runningHeadless">If set to <c>false</c> do not bind to console</param>
		public Atlas(int port, bool runningHeadless) {
			this.runningHeadless = runningHeadless;

			var config = new NetPeerConfiguration("Atlas");
			config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
			config.Port = port;
			server = new NetServer(config);
			server.Start();
            enemys.Add(new ServerEnemy(0, 128, 32 * 2, ServerEnemy.Type.Mage));
		}

		/// <summary>
		/// Start the main server loop
		/// </summary>
		public void Run() {
			if(runningHeadless) {
				var inputThread = new Thread(InputHandler);
				inputThread.Start();                
			}
			lastUpdates = 0;
			while(!isExiting) {
				lastTime = currentTime;
				currentTime = DateTime.UtcNow.Ticks;

				if(currentTime - lastUpdates <= (TimeSpan.TicksPerSecond / 120)) continue;
				#region Incoming messages
				NetIncomingMessage inMsg;
				while((inMsg = server.ReadMessage()) != null) {
					switch(inMsg.MessageType) {
						case NetIncomingMessageType.Data:
							 switch((AtlasPackets)inMsg.ReadByte()) {
                                 case AtlasPackets.RequestPositionChange: {
                                        short X = inMsg.ReadInt16();
                                        short Y = inMsg.ReadInt16();
                                        string Name = inMsg.ReadString();
                                        long ID = inMsg.SenderConnection.RemoteUniqueIdentifier;
                                        players[ID].MoveVector = new Point(X, Y);
                                        players[ID].Name = Name;
                                        //players[ID].pColor = new Color(R, G, B);
                                    } break;
                                case AtlasPackets.RequestMoveVector: {
                                        int ID = inMsg.ReadInt32();
                                        int X = inMsg.ReadInt32();
                                        int Y = inMsg.ReadInt32();
                                        enemys[ID].Target = new Point(X, Y);
                                    } break;
							 }
							break;
						case NetIncomingMessageType.StatusChanged:
							switch(inMsg.SenderConnection.Status) {
								case NetConnectionStatus.Connected:
									players.Add(inMsg.SenderConnection.RemoteUniqueIdentifier, new DebugPlayer(inMsg.SenderConnection.RemoteUniqueIdentifier, 0, 0, 10));
                                    break;
								case NetConnectionStatus.Disconnected:
									players.Remove(inMsg.SenderConnection.RemoteUniqueIdentifier);
									break;
							}
							break;
						case NetIncomingMessageType.DiscoveryRequest:
							//TODO: FIX DIS
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

				//Parallel.ForEach(players.Values, PlayerUpdate);
                
                foreach (var p in players) {
                    PlayerUpdate(p.Value, enemys.ToList());
                }
                foreach (var e in enemys) {
                    EnemyUpdate(e, players.Values.ToList());
                }
                enemys.AddRange(addEnemys);
                addEnemys.Clear();

                #region Outgoing Updates
                //TODO: Compute changed frames, keyframes, etc
                NetOutgoingMessage outMsg = server.CreateMessage();
				outMsg.Write((byte)AtlasPackets.UpdatePositions);
				outMsg.Write((ushort)players.Count);
				foreach (var p in players.Values) {
					outMsg.Write(p.NetID);
					outMsg.Write((int)p.Position.X);
					outMsg.Write((int)p.Position.Y);
                    outMsg.Write(p.Name);
                    outMsg.Write(p.Health);
                    outMsg.Write(p.mult);
                    outMsg.Write(p.currentFrame);
				}
				server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);

                outMsg = server.CreateMessage();
                outMsg.Write((byte)AtlasPackets.UpdateEnemy);
                outMsg.Write((ushort)enemys.Count);
                foreach (var e in enemys) {
                    outMsg.Write(e.NetID);
                    outMsg.Write((int)e.Position.X);
                    outMsg.Write((int)e.Position.Y);
                    outMsg.Write((int)e.entType);
                    outMsg.Write(e.currentFrame);
                    outMsg.Write(e.mult);
                }
                server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);

				lastUpdates = currentTime;
				#endregion
			}
			server.Shutdown("Bye!");
		}

		/// <summary>
		/// Handles the input from the console
		/// </summary>
		void InputHandler() {
			while(!isExiting) {
				//Timing not needed, as ReadLine() locks the execution pointer
				string readLine = Console.ReadLine();
				#region Parse out the command and arguments
				//If they didn't say anything, we don't need to do anything
				if(string.IsNullOrWhiteSpace(readLine)) return;

				string command;
				#region Parse out parameterless commands
				try {
					//The command is anything that happens before the opening parenthesies
					command = readLine.Substring(0, readLine.IndexOf('('));
				}
				catch (Exception) {
					//If there isn't an opening parenthesies, it's a parameterless command
					command = readLine;
				}
				#endregion

				var args = new List<string>();
                string argStr = string.Empty;
                //Take everything between the opening and closing parenthesies
                try {
                    argStr = readLine.Substring(readLine.IndexOf('(') + 1, readLine.IndexOf(')') - (readLine.IndexOf('(') + 1));
                }
                catch (Exception e) {
                }
				try {
					//Try to split the arguments by commas
					args = argStr.Split(',').ToList();
				}
				catch (Exception) {
					try {
						//If the split failed, it ether had no arguments (do nothing)
						//or had only one argument (add that argument)
						if(!string.IsNullOrWhiteSpace(argStr))
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
				catch (NotSupportedException e) {
					Console.WriteLine(e.Message);
				}
			}
		}

		/// <summary>
		/// Control the server instance
		/// </summary>
		/// <param name="command">Command to be exected</param>
		/// <param name="args">Arguments being passed in</param>
		public void Control(string command, List<string> args) {
			switch(command.ToUpper()) {
				case "END":
				case "STOP":
				case "EXIT":
					isExiting = true;
					break;
				case "CLEAR":
				case "CLS":
					Console.Clear();
					break;
				case "ID":
					foreach (var p in players) {
						Console.WriteLine("ID: {0}\n\tPosition:({1},{2})\n\tMoving To:({3},{4})", p.Key, p.Value.Position.X, p.Value.Position.Y, p.Value.MovingTo.X, p.Value.MovingTo.Y);
					}
					break;
				case "ADDENT":
					if(args.Capacity > 0) {
						enemys.Add(new ServerEnemy((long)enemys.Count + 1, int.Parse(args[0]), int.Parse(args[1]), ServerEnemy.Type.Debug));
                    }
                    break;
                case "ENTS":
                    foreach(var e in enemys)
                        Console.WriteLine("ID: {0}\n\tPosition:({1},{2})", e.NetID, e.Position.X, e.Position.Y);
                    break;
				case "PLAYERS":
                    foreach(var e in players)
                        Console.WriteLine("ID: {0}\n\tPosition:({1},{2})\n\tMoving To:({3},{4})");
                    break;
				case "MOVE": {
						var location = new Vector2(float.Parse(args[1]), float.Parse(args[2]));
						Console.WriteLine("moving to " + location);
						long ID = long.Parse(args[0]);
						if(players.ContainsKey(ID))
							players[Convert.ToInt64(args[0])].MovingTo = location;
						break; 
					}
				default:
					{
						string argList = string.Empty;
						args.ForEach(arg => argList += arg + ",");
						throw new NotSupportedException(string.Format("Unrecognised command\nCommand: {0}\nArgs: {1}", command, argList));
					}
			}
		}
	}
}

