using System;
using System.Collections.Generic;
using MirRemakeBackend.DataEntity;
using MirRemakeBackend.Entity;
using MirRemakeBackend.Network;
using MirRemakeBackend.Util;

namespace MirRemakeBackend.GameLogic {
    partial class GL_UnitBattleAttribute {
        private class EffectCalculateStage {
            private DE_Effect m_de;
            private short m_animId;
            public bool m_hit;
            public bool m_critical;
            public int m_deltaHp;
            public int m_deltaMp;
            public (short, float, float) [] m_statusIdAndValueAndTimeArr;
            public float m_hatred;
            public void InitWithCasterAndTarget (DE_Effect effectDe, short animId, E_Unit caster, E_Unit target) {
                m_de = effectDe;
                m_animId = animId;
                // 计算命中
                float hitRate = (100 + effectDe.m_hitRate) * caster.m_HitRate * 0.01f;
                m_hit = MyRandom.NextInt (1, 101) <= hitRate;
                if (m_hit) {
                    // 计算基础伤害 (或能量剥夺)
                    m_deltaHp = effectDe.m_deltaHp;
                    m_deltaMp = effectDe.m_deltaMp;
                    for (int i = 0; i < effectDe.m_attrBonus.Count; i++) {
                        switch (effectDe.m_attrBonus[i].Item1) {
                            case ActorUnitConcreteAttributeType.ATTACK:
                                m_deltaHp = m_deltaHp + (int) (caster.m_Attack * effectDe.m_attrBonus[i].Item2);
                                break;
                            case ActorUnitConcreteAttributeType.MAGIC:
                                m_deltaHp = m_deltaHp + (int) (caster.m_Magic * effectDe.m_attrBonus[i].Item2);
                                break;
                            case ActorUnitConcreteAttributeType.MAX_HP:
                                m_deltaHp = m_deltaHp + (int) (caster.m_MaxHp * effectDe.m_attrBonus[i].Item2);
                                break;
                            case ActorUnitConcreteAttributeType.MAX_MP:
                                m_deltaHp = m_deltaHp + (int) (caster.m_MaxMp * effectDe.m_attrBonus[i].Item2);
                                break;
                        }
                    }
                    // 计算暴击
                    float criticalRate = effectDe.m_criticalRate * caster.m_CriticalRate * 0.01f;
                    float bonus=criticalRate>1?criticalRate-1:0;
                    m_critical = MyRandom.NextInt (1, 101) <= criticalRate;
                    if (m_critical)
                        m_deltaHp = (int) (m_deltaHp * (1f + (float) caster.m_CriticalBonus * 0.01f+2*bonus));
                    if(m_deltaHp<0){
                        switch (effectDe.m_type) {
                            case EffectType.PHYSICS:
                                m_deltaHp = GetDamage(m_deltaHp,target.m_Defence);
                                break;
                            case EffectType.MAGIC:
                                m_deltaHp = GetDamage(m_deltaHp,target.m_Resistance);
                                break;
                        }
                    }
                    // 减伤+易伤 
                    if (m_deltaHp < 0) {
                        if (effectDe.m_type == EffectType.PHYSICS) {
                            m_deltaHp = (int) (m_deltaHp * (target.m_PhysicsVulernability * 0.01f + 1) * (1 - target.m_DamageReduction * 0.01f));
                        } else {
                            m_deltaHp = (int) (m_deltaHp * (target.m_MagicVulernability * 0.01f + 1) * (1 - target.m_DamageReduction * 0.01f));
                        }

                    }

                    // 计算状态
                    m_statusIdAndValueAndTimeArr = new (short, float, float) [effectDe.m_statusIdAndValueAndTimeList.Count];
                    for (int i = 0; i < effectDe.m_statusIdAndValueAndTimeList.Count; i++) {
                        var info = effectDe.m_statusIdAndValueAndTimeList[i];
                        float value = info.Item2 / target.m_Tenacity;
                        float durationTime = info.Item3 / target.m_Tenacity;
                        m_statusIdAndValueAndTimeArr[i] = (info.Item1, value, durationTime);
                    }

                    // xjb计算仇恨
                    if (m_deltaHp < 0)
                        m_hatred = -m_deltaHp;
                }
            }
            public NO_Effect GetNo () {
                return new NO_Effect (m_animId, m_hit, m_critical, m_deltaHp, m_deltaMp);
            }
             //计算护甲和魔法抗性的减伤
             private int GetDamage (int damage, int armor) {
                if(armor<=0) return damage;
                 if (armor <= 100) {
                     return (int) (damage * (1 - armor / (armor + 100.0)));
                }
                 if (armor <= 1000) {
                     return (int) (damage * (0.5 - armor / (armor + 1000.0)));
                 }
                 return (int) (damage * (0.25 - armor / (armor + 10000.0)));
             }
        }
    }
}