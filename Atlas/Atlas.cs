using System;
using System.Threading;
using System.Collections.Generic;
using Lidgren.Network;
using System.Linq;
using FlatBuffers;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Edge.Atlas.Components;
using Buffers = Edge.NetCommon.Atlas;

namespace Edge.Atlas {
	public partial class Atlas {
		NetServer server;
		public Boolean isExiting;
		Boolean runningHeadless;
		public Dictionary<Int64,Entity> entities = new Dictionary<Int64,Entity>();
		public List<Projectile> projectiles = new List<Projectile>();

		public Int64 lastTime;
		public Int64 currentTime = DateTime.UtcNow.Ticks;


		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args) {
			if(args.Length > 0)
				try {
					new Atlas(Int32.Parse(args[0]), false).Run();
				}
				catch(Exception) {
					new Atlas(2348, false).Run();
				}
			else
				new Atlas(2348, true).Run();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Edge.Atlas.Atlas"/> class.
		/// </summary>
		/// <param name="port">Port.</param>
		/// <param name="runningHeadless">If set to <c>false</c> do not bind to console</param>
		public Atlas(Int32 port, Boolean runningHeadless) {
			this.runningHeadless = runningHeadless;

			var config = new NetPeerConfiguration("Atlas");
			config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
			config.Port = port;
			server = new NetServer(config);
			server.Start();
		}

		/// <summary>
		/// Start the main server loop
		/// </summary>
		public void Run() {
			if(runningHeadless) {
				var inputThread = new Thread(InputHandler);
				inputThread.Start();                
			}
			Int64 lastUpdates = 0;
			while(!isExiting) {
				lastTime = currentTime;
				currentTime = DateTime.UtcNow.Ticks;
				var deltaTime = currentTime - lastTime;

				if(currentTime - lastUpdates <= (TimeSpan.TicksPerSecond / 120)) continue;
				#region Incoming messages
				NetIncomingMessage inMsg;
				while((inMsg = server.ReadMessage()) != null) {
					switch(inMsg.MessageType) {
						case NetIncomingMessageType.Data:
							var buff = new ByteBuffer(inMsg.ReadBytes(inMsg.LengthBytes));
							var packet = Buffers.Packet.GetRootAsPacket(buff);
							switch(packet.DataType) {
								case Buffers.PacketData.MoveEvent:
									var moveEvent = new Buffers.MoveEvent();
									packet.GetData<Buffers.MoveEvent>(moveEvent);

									throw new Exception("Player Moved" + moveEvent.Delta.X + moveEvent.Delta.Y);
									break;
								case Buffers.PacketData.AbilityEvent:
									var abilityEvent = new Buffers.AbilityEvent();
									packet.GetData<Buffers.AbilityEvent>(abilityEvent);
									string s = abilityEvent.Id.ToString();
									switch(abilityEvent.TargetType) {
										case Buffers.AbilityTarget.Vector2:
											{
												var target = new Buffers.Vector2();
												abilityEvent.GetTarget<Buffers.Vector2>(target);
												s = "Player abilitied" + target.X + target.Y;
											}
											break;
										case Buffers.AbilityTarget.EntityReference:
											{
												var target = new Buffers.EntityReference();
												abilityEvent.GetTarget<Buffers.EntityReference>(target);
												s = "Player abilitied" + target.Id;
											}
											break;
									}

									throw new Exception(s);
									break;
							}
						case NetIncomingMessageType.StatusChanged:
							switch(inMsg.SenderConnection.Status) {
								case NetConnectionStatus.Connected:
									entities.Add(inMsg.SenderConnection.RemoteUniqueIdentifier, new HumanPlayer(inMsg.SenderConnection.RemoteUniqueIdentifier));
									break;
								case NetConnectionStatus.Disconnected:
									entities.Remove(inMsg.SenderConnection.RemoteUniqueIdentifier);
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

				var fbb = new FlatBufferBuilder(1); //TODO: RESIZE THIS CLOSER TO WHAT IT ACTUALLY IS
				var eo = new Offset<Buffers.Entity>[entities.Count];

				for(int i = 0, count = entities.Values.Count; i < count; i++) {
					Entity e = entities[entities.Keys[i]];
					e.Update(deltaTime);
					eo[i] = e.ToBuffer(fbb);
				}
				var po = new Offset<Buffers.Projectile>[projectiles.Count];
				for(int i = 0, projectilesCount = projectiles.Count; i < projectilesCount; i++) {
					var p = projectiles[i];
					po[i] = p.ToBuffer(fbb);
				}
				var pulse = Buffers.EntityPulse.CreateEntityPulse(fbb, Buffers.EntityPulse.CreateEntitiesVector(fbb, eo.ToArray()), Buffers.EntityPulse.CreateProjectilesVector(fbb, po));
				var finalOffset = Buffers.Packet.CreatePacket(fbb, Buffers.PacketData.EntityPulse, pulse.Value);
				Buffers.Packet.FinishPacketBuffer(fbb, finalOffset);
				#region Outgoing Updates
				/*TODO: Compute changed frames, keyframes, etc
				 * Right, what we could do for that is have a hashing dictionary that keeps track of a hash of each entry 
				 * (GetHashCode or something idk)
				 * 
				 * and then only run the update method if they're different, 
				 * 
				 * but we'd either need to be able to deep clone the dictionary, 
				 * or make it so we can have a deepish clone of just the hashes and IDs for stuff
				 * which is prob the better solution tbh
				 */
				NetOutgoingMessage outMsg = server.CreateMessage();
				/* AIGHT PEOPLE
				 * HERE'S WHAT WE'RE GONNA DO
				 * 
				 * MAKE A FLATBUFFER (AN ENTITYPULSE TO BE SPECIFIC)
				 * AND WE'RE GONNA SEND IT TO EVERYONE
				 */
				byte[] data;
				using(var ms = new System.IO.MemoryStream(fbb.DataBuffer.Data, fbb.DataBuffer.Position, fbb.Offset))
					data = ms.ToArray();
				outMsg.Write(data);
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
				if(String.IsNullOrWhiteSpace(readLine)) return;

				string command;
				#region Parse out parameterless commands
				try {
					//The command is anything that happens before the opening parenthesies
					command = readLine.Substring(0, readLine.IndexOf('('));
				}
				catch(Exception) {
					//If there isn't an opening parenthesies, it's a parameterless command
					command = readLine;
				}
				#endregion

				var args = new List<String>();
				//Take everything between the opening and closing parenthesies
				String argStr = readLine.Substring(readLine.IndexOf('(') + 1, readLine.IndexOf(')') - (readLine.IndexOf('(') + 1));
				try {
					//Try to split the arguments by commas
					args = argStr.Split(',').ToList();
				}
				catch(Exception) {
					try {
						//If the split failed, it ether had no arguments (do nothing)
						//or had only one argument (add that argument)
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

		/// <summary>
		/// Control the server instance
		/// </summary>
		/// <param name="command">Command to be exected</param>
		/// <param name="args">Arguments being passed in</param>
		public void Control(String command, List<String> args) {
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
					foreach(var p in players) {
						Console.WriteLine("ID: {0}\n\tPosition:({1},{2})\n\tMoving To:({3},{4})", p.Key, p.Value.Position.X, p.Value.Position.Y, p.Value.MovingTo.X, p.Value.MovingTo.Y);
					}
					break;
				case "ADDENT":
					if(args.Capacity > 0) {
						entities.Add(new Entity(long.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2])));

					}
					break;
				case "ENTS":
					foreach(var e in entities)
						Console.WriteLine("ID: {0}\n\tPosition:({1},{2})\n\tMoving To:({3},{4})");
					break;
				case "PLAYERS":
					foreach(var e in players)
						Console.WriteLine("ID: {0}\n\tPosition:({1},{2})\n\tMoving To:({3},{4})");
					break;
				case "MOVE":
					{
						var location = new Vector2(float.Parse(args[1]), float.Parse(args[2]));
						Console.WriteLine("moving to " + location);
						Int64 ID = Int64.Parse(args[0]);
						if(players.ContainsKey(ID))
							players[Convert.ToInt64(args[0])].MovingTo = location;
						break; 
					}
				default:
					{
						String argList = String.Empty;
						args.ForEach(arg => argList += arg + ",");
						throw new NotSupportedException(String.Format("Unrecognised command\nCommand: {0}\nArgs: {1}", command, argList));
					}
			}
		}
	}
}

