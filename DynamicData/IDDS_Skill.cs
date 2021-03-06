using System.Collections.Generic;

namespace MirRemakeBackend.DynamicData {
    interface IDDS_Skill {
        List<DDO_Skill> GetSkillListByCharacterId (int charId);
        void InsertSkill (DDO_Skill ddo);
        void UpdateSkill (DDO_Skill ddo);
    }
}