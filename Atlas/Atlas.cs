using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Lidgren.Network;
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
			config.Port = port;
			server = new NetServer(config);
			server.Start();
		}

		public void Run() {
			#region What we're currently using (but shouldn't be)
			Task.Run(NetworkIncomingLoop);
			if(runningHeadless)
				LogicLoop();
			else {
				Task.Run(LogicLoop);
				while(!isExiting) {
					//do the input handler thingy
				}
			}
			#endregion

			server.Shutdown("Bye!");
		}
		#region IGNORE THIS
		void LogicLoop() {
			while(!isExiting) {
				lastTime = currentTime;
				currentTime = DateTime.UtcNow.Ticks;
				foreach(var x in players.Values)
					PlayerUpdate(x);
				SendUpdates();
			}

		}
		#endregion
		public void Control(String command, List<String> args) {
			//allows Maestro (or other apps) to control headless instances
		}
	}
}

