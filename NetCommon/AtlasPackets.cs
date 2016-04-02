using System;

namespace Edge.NetCommon {
	public enum AtlasPackets:int {
		#region
		RequestPositionChange,
        RequestJumpChange,
        RequestMoveVector,
        RequestAnimation,
        UpdateMoveVector,
		UpdatePositions,
        UpdateAnimation,
        UpdateName,
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

