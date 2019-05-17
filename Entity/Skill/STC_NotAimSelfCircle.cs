using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirRemakeBackend {
    /// <summary>
    /// 技能目标选择器  
    /// 目标阵营: 友方, 敌方  
    /// 瞄准类型有: 自身, 其他Unit, 非锁定型  
    /// 范围类型有: 直线型, 圆型  
    /// 目标数量  
    /// </summary>
    class STC_NotAimSelfCircle : SkillTargetChooserBase {
        // 技能释放点类型
        public override SkillAimType m_TargetAimType { get { return SkillAimType.NOT_AIM_SELF_CIRCLE; } }
        // 射程
        public float m_castRange;
        // 伤害半径
        public float m_damageRange;
        public STC_NotAimSelfCircle () {
            // TODO: 仅用于测试, 日后应当删除
            m_targetCamp = CampType.ENEMY;
            m_targetNumber = 3;
            m_castRange = 3.0f;
            m_damageRange = 1.0f;
        }
        public override SkillParam CompleteSkillParam (E_ActorUnit self, E_ActorUnit aimedTarget, SkillParam parm) {
            if (aimedTarget != null && !parm.m_isValid)
                return new SkillParam (m_TargetAimType, null, Vector2.zero, aimedTarget.m_Position);
            return parm;
        }
        public override bool InRange (Vector2 pos, SkillParam tarPos) {
            return true;
        }
        public override List<E_ActorUnit> GetEffectTargets (E_ActorUnit self, SkillParam parm) {
            // TODO: 
            return null;
        }
    }
}