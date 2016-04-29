using System;

namespace Edge.NetCommon {
	public enum AtlasPackets:int {
		#region
		RequestPositionChange,
        RequestJumpChange,
        RequestMoveVector,
        RequestAnimation,
        RequestItem,
        RequestWall,
        UpdateMoveVector,
		UpdatePositions,
        UpdateAnimation,
        UpdateName,
        UpdateItem,
        UpdateWall,
		#endregion

		#region Server to Client
        SendChatMessage,
        UpdateEnemy,
        #endregion

        #region Client to Server
        DamageEnemy
        #endregion
    }
}

