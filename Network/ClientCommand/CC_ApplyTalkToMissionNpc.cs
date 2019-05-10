using System;
using UnityEngine;
using LiteNetLib.Utils;

namespace MirRemake {
    class CC_ApplyTalkToMissionNpc : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_TALK_TO_MISSION_NPC; } }
        public void Execute(NetDataReader reader, int netId) {
            short npcId = reader.GetShort ();
            SM_ActorUnit.s_instance.CommandTalkToMissionNpc (netId, npcId);
        }
    }
}