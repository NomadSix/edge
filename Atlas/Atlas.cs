using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Net;
using System.Linq;
using Edge.NetCommon;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

using Type = Edge.NetCommon.Type;

namespace Edge.Atlas {
    public partial class Atlas {
        NetServer server;
        bool runningHeadless;
        public bool isExiting;
        public Dictionary<long, DebugPlayer> players = new Dictionary<long, DebugPlayer>();
        public List<ServerEnemy> enemys = new List<ServerEnemy>();
        public List<Item> items = new List<Item>();

        public List<DebugPlayer> addPlayers = new List<DebugPlayer>();
        public List<ServerEnemy> addEnemys = new List<ServerEnemy>();
        public List<Item> addItem = new List<Item>();

        public List<ServerEnemy> removeEnemys = new List<ServerEnemy>();
        public List<long> removePlayers = new List<long>();
        public List<Item> removeItems = new List<Item>();

        public Rectangle[] walls = new Rectangle[] {
            new Rectangle(0, 72, 1536, 16),
            new Rectangle(34, 0, 16, 1536),
            new Rectangle(1487, 0, 16, 1536),
            new Rectangle(0, 1438, 1536, 16)
    };

        public long lastTime;
        public long lastUpdates;
        public long currentTime = DateTime.UtcNow.Ticks;

        readonly int MAX_ENEMYS = 60;
        readonly int MIN_ENEMYS = 30;
        readonly int SPAWNAREAR = 10;


        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args) {
            if (args.Length > 0)
                try {
                    new Atlas(int.Parse(args[0]), true).Run();
                } catch (Exception) {
                    new Atlas(2348, true).Run();
                } else
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
            items.Add(new Item(0, 100, 100, Item.Type.Health));
        }

        /// <summary>
        /// Start the main server loop
        /// </summary>
        public void Run() {
            if (runningHeadless) {
                var inputThread = new Thread(InputHandler);
                inputThread.Start();
            }
            lastUpdates = 0;
            while (!isExiting) {
                lastTime = currentTime;
                currentTime = DateTime.UtcNow.Ticks;

                if (currentTime - lastUpdates <= (TimeSpan.TicksPerSecond / 120)) continue;
                #region Incoming messages
                NetIncomingMessage inMsg;
                while ((inMsg = server.ReadMessage()) != null) {
                    switch (inMsg.MessageType) {
                        case NetIncomingMessageType.DiscoveryRequest:
                            //TODO: FIX DIS
                            server.SendDiscoveryResponse(null, inMsg.SenderEndPoint);
                            break;
                        case NetIncomingMessageType.Data:
                            switch ((AtlasPackets)inMsg.ReadByte()) {
                                case AtlasPackets.RequestPositionChange: {
                                        short X = inMsg.ReadInt16();
                                        short Y = inMsg.ReadInt16();
                                        string Name = inMsg.ReadString();
                                        bool Attack = inMsg.ReadBoolean();
                                        long ID = inMsg.SenderConnection.RemoteUniqueIdentifier;
                                        players[ID].MoveVector = new Point(X, Y);
                                        players[ID].Name = Name;
                                        players[ID].isAttacking = Attack;
                                        //players[ID].pColor = new Color(R, G, B);
                                    }
                                    break;
                                case AtlasPackets.RequestMoveVector: {
                                        int ID = inMsg.ReadInt32();
                                        int X = inMsg.ReadInt32();
                                        int Y = inMsg.ReadInt32();
                                        enemys[ID].Target = new Point(X, Y);
                                    }
                                    break;
                                case AtlasPackets.RequestItem: {
                                        int ID = inMsg.ReadInt32();
                                        int X = inMsg.ReadInt32();
                                        int Y = inMsg.ReadInt32();
                                        items[ID].Hitbox = new Rectangle(X, Y, 8, 8);
                                    }
                                    break;
                            }
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            switch (inMsg.SenderConnection.Status) {
                                case NetConnectionStatus.Connected:
                                    addPlayers.Add(new DebugPlayer(inMsg.SenderConnection.RemoteUniqueIdentifier, 0,0,2));
                                    break;
                                case NetConnectionStatus.Disconnected:
                                    players.Remove(inMsg.SenderConnection.RemoteUniqueIdentifier);
                                    break;
                            }
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
                    PlayerUpdate(p.Value, enemys);
                }
                for (int i = 0; i < items.Count; i++) {
                    ItemUpdate(items[i]);
                }
                for (int e = 0; e < enemys.Count; e++) {
                    EnemyUpdate(enemys[e]);
                }
                for (int p = 0; p < removePlayers.Count; p++)
                {
                    var player = players[removePlayers[p]];
                    players.Remove(removePlayers[p]);
                    items.Add(new Item(items.Count + 1, player.Hitbox.X - player.Hitbox.Width / 2, player.Hitbox.Y, Item.Type.Gold));
                    items.Add(new Item(items.Count + 1, player.Hitbox.X - player.Hitbox.Width / 3, player.Hitbox.Y - player.Hitbox.Height / 2, Item.Type.Gold));
                    items.Add(new Item(items.Count + 1, player.Hitbox.X, player.Hitbox.Y + player.Hitbox.Width / 2, Item.Type.Gold));
                }
                for (int e = 0; e < removeEnemys.Count; e++) {
                    enemys.Remove(removeEnemys[e]);
                }
                for (int i = 0; i < removeItems.Count; i++) {
                    items.Remove(removeItems[i]);
                }
                for (int p = 0; p < addPlayers.Count; p++) {
                    var ID = addPlayers[p].NetID;
                    players.Add(ID, new DebugPlayer(ID, NetRandom.Instance.Next(768 - SPAWNAREAR, 768 + SPAWNAREAR), NetRandom.Instance.Next(768 - SPAWNAREAR, 768 + SPAWNAREAR), 2f));
                }
                for (int e = 0; e < addEnemys.Count; e++) {
                    if (enemys.Count >= MAX_ENEMYS) break;
                    enemys.Add(addEnemys[e]);
                }

                addEnemys.Clear();
                addPlayers.Clear();
                removeEnemys.Clear();
                removePlayers.Clear();
                removeItems.Clear();

                if (enemys.Count < MIN_ENEMYS) {
                    addEnemys.Add(new ServerEnemy(enemys.Count + 1, NetRandom.Instance.Next(100, 1400), NetRandom.Instance.Next(100, 1400), (Type)NetRandom.Instance.Next(1,5)));
                }

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
                    outMsg.Write(p.isAttacking);
                    outMsg.Write(p.gold);
                    outMsg.Write(p.lifeTimer);
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

                outMsg = server.CreateMessage();
                outMsg.Write((byte)AtlasPackets.UpdateItem);
                outMsg.Write((ushort)items.Count);
                foreach (var i in items) {
                    outMsg.Write(i.ID);
                    outMsg.Write((int)i.Position.X);
                    outMsg.Write((int)i.Position.Y);
                    outMsg.Write((int)i.type);
                }
                server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);

                outMsg = server.CreateMessage();
                outMsg.Write((byte)AtlasPackets.UpdateWall);
                outMsg.Write(walls.Length);
                foreach (var i in walls) {
                    outMsg.Write(i.X);
                    outMsg.Write(i.Y);
                    outMsg.Write(i.Width);
                    outMsg.Write(i.Height);
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
            while (!isExiting) {
                //Timing not needed, as ReadLine() locks the execution pointer
                string readLine = Console.ReadLine();
                #region Parse out the command and arguments
                //If they didn't say anything, we don't need to do anything
                if (string.IsNullOrWhiteSpace(readLine)) return;

                string command;
                #region Parse out parameterless commands
                try {
                    //The command is anything that happens before the opening parenthesies
                    command = readLine.Substring(0, readLine.IndexOf('('));
                } catch (Exception) {
                    //If there isn't an opening parenthesies, it's a parameterless command
                    command = readLine;
                }
                #endregion

                var args = new List<string>();
                string argStr = string.Empty;
                //Take everything between the opening and closing parenthesies
                try {
                    argStr = readLine.Substring(readLine.IndexOf('(') + 1, readLine.IndexOf(')') - (readLine.IndexOf('(') + 1));
                } catch (Exception e) {
                }
                try {
                    //Try to split the arguments by commas
                    args = argStr.Split(',').ToList();
                } catch (Exception) {
                    try {
                        //If the split failed, it ether had no arguments (do nothing)
                        //or had only one argument (add that argument)
                        if (!string.IsNullOrWhiteSpace(argStr))
                            args.Add(argStr);
                    }
                    // Analysis disable once EmptyGeneralCatchClause
                    catch {
                    }
                }
                #endregion
                try {
                    Control(command, args);
                } catch (NotSupportedException e) {
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
            switch (command.ToUpper()) {
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
                    if (args.Capacity > 0) {
                        enemys.Add(new ServerEnemy((long)enemys.Count + 1, int.Parse(args[0]), int.Parse(args[1]), (Type)int.Parse(args[2])));
                    }
                    break;
                case "ENTS":
                    for (int i = 0; i < enemys.Count; i++) {
                        ServerEnemy e = enemys[i];
                        Console.WriteLine("ID: {0}\n\tPosition:({1},{2})", e.NetID, e.Position.X, e.Position.Y);
                    }
                    break;
                case "PLAYERS":
                    foreach (var e in players)
                        Console.WriteLine("ID: {0}\n\tPosition:({1},{2})\n\tMoving To:({3},{4})");
                    break;
                case "MOVE": {
                        var location = new Vector2(float.Parse(args[1]), float.Parse(args[2]));
                        Console.WriteLine("moving to " + location);
                        long ID = long.Parse(args[0]);
                        if (players.ContainsKey(ID))
                            players[Convert.ToInt64(args[0])].MovingTo = location;
                        break;
                    }
                default: {
                        string argList = string.Empty;
                        args.ForEach(arg => argList += arg + ",");
                        throw new NotSupportedException(string.Format("Unrecognised command\nCommand: {0}\nArgs: {1}", command, argList));
                    }
            }
        }
    }
}

