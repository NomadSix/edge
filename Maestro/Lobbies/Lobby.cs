using System;
using System.Collections.Generic;

namespace Edge.Maestro.Lobbies {
	public class Lobby {
		public Int32 LobbyUID;
		public String Host;
		public Byte PlayersPerTeam;
		public List<Participant> Members;

		public Lobby(Int32 UID, Byte playersPerTeam, String host) {
			LobbyUID = UID;
			PlayersPerTeam = playersPerTeam;
			Host = host;
			Members = new List<Participant>();
			Members.Add(new Participant(host, true));
		}
	}

}