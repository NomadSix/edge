using System;

namespace Edge.Maestro.Lobbies {
	public class Participant {
		public Boolean IsResponded;
		public Boolean IsConnected;
		public String UName;

		public Participant(String name, Boolean isConnected) {
			IsConnected = isConnected;
			UName = name;
		}

		public Participant(String name) {
			IsConnected = false;
			UName = name;
		}
	}
}

