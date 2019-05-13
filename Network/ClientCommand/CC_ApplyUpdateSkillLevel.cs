using LiteNetLib.Utils;

namespace MirRemake {
    class CC_ApplyUpdateSkillLevel : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_UPDATE_SKILL_LEVEL; } }
        public void Execute (NetDataReader reader, int netId) {
            short skillId = reader.GetShort ();
            short targetSkillLevel = reader.GetShort ();
            SM_ActorUnit.s_instance.CommandUpdateSkillLevel (netId, skillId, targetSkillLevel);
        }
    }
}