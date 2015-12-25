using System;

namespace Edge.NetCommon {
	public enum AtlasPackets:int {
		#region
		RequestPositionChange,
        RequestJumpChange,
        RequestMoveVector,
        UpdateMoveVector,
		UpdatePositions,
        UpdateName,
		#endregion

		#region Server to Client
        SendChatMessage
		#endregion

		#region Client to Server
		#endregion
	}
}

