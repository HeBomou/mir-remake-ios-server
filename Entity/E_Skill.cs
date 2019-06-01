using System;
using System.Collections.Generic;
using MirRemakeBackend.DataEntity;
using MirRemakeBackend.DynamicData;
using MirRemakeBackend.Util;

namespace MirRemakeBackend.Entity {
    class E_MonsterSkill {
        public DE_Skill m_skillDe;
        public DE_SkillData m_skillDataDe;
        public short m_skillId;
        public short m_level;
        public MyTimer.Time m_lastCastTime;
        public CampType m_TargetCamp { get { return m_skillDe.m_targetCamp; } }
        public byte m_TargetNumber { get { return m_skillDataDe.m_targetNumber; } }
        public float m_SingAndCastFrontTime { get { return m_skillDataDe.m_singTime + m_skillDataDe.m_castFrontTime; } }
        public float m_CastBackTime { get { return m_skillDataDe.m_castBackTime; } }
        public SkillAimType m_AimType { get { return m_skillDe.m_skillAimType; } }
        public float m_CastRange { get { return m_skillDataDe.m_castRange; } }
        public IReadOnlyList<ValueTuple<SkillAimParamType, float>> m_DamageParamList { get { return m_skillDataDe.m_damageParamList; } }
        public DE_Effect m_SkillEffect { get { return m_skillDataDe.m_skillEffect; } }
        public bool m_IsCoolingDown { get { return m_lastCastTime.Ticked (m_skillDataDe.m_coolDownTime) >= MyTimer.s_CurTime; } }

        public E_MonsterSkill () { }
        public E_MonsterSkill (short skillId, short skillLv, DE_Skill de, DE_SkillData dataDe) {
            m_skillId = skillId;
            m_level = skillLv;
            m_skillDe = de;
            m_skillDataDe = dataDe;
        }
        public void StartCoolDown () {
            m_lastCastTime = MyTimer.s_CurTime;
        }
    }
    class E_Skill : E_MonsterSkill {
        // 技能熟练度
        public int m_masterly;
        public E_Skill () { }
        public void Reset (DE_Skill skillDe, DE_SkillData skillDataDe, DDO_Skill ddo) {
            m_skillDe = skillDe;
            m_skillDataDe = skillDataDe;
            m_skillId = ddo.m_skillId;
            m_level = ddo.m_skillLevel;
            m_masterly = ddo.m_masterly;
        }
        public void Upgrade () {
            if (m_level >= m_skillDe.m_skillMaxLevel) return;
            m_level++;
            m_masterly -= m_skillDataDe.m_upgradeMasterlyInNeed;
            m_skillDataDe = m_skillDe.m_skillDataAllLevel[m_level - 1];
        }
    }
}