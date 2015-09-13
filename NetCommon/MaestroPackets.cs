using System;

namespace Edge.NetCommon {
	public enum MaestroPackets:byte {
		//Session Creation and Authentication
		//Put the authentication things here

		//Lobby
		CreateLobby,
		InviteToLobby,
		ReplyToLobbyInvite,
		LobbyStatus,
		StartLobby,
		IntroduceAtlas
	}
}

