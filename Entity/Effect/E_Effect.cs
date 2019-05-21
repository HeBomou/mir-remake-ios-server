/**
 * Enity，使用物品或技能等对角色属性造成变化的实体
 * 创建者 fn
 * 时间 2019/4/1
 * 最后修改者 yuk
 * 时间 2019/4/3
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirRemakeBackend {
    struct E_Effect {
        public DE_Effect m_dataEntity;
        public int m_casterNetworkId;
        public float m_hitRate;
        public float m_criticalRate;
        public int m_deltaHp;
        public int m_deltaMp;
        public E_Status[] m_statusAttachArray;
        public int m_StatusAttachNum { get { return m_statusAttachArray.Length; } }

        public E_Effect (DE_Effect effectDe, int casterNetId) {
            m_dataEntity = effectDe;
            m_casterNetworkId = casterNetId;
            m_hitRate = effectDo.m_hitRate;
            m_criticalRate = effectDo.m_criticalRate;
            m_deltaHp = effectDo.m_deltaHP;
            m_deltaMp = effectDo.m_deltaMP;
            DO_Status[] statusDoArr = effectDo.m_statusAttachArray;
            m_statusAttachArray = new E_Status[statusDoArr.Length];
            for (int i=0; i<statusDoArr.Length; i++)
                m_statusAttachArray[i] = new E_Status (statusDoArr[i], casterNetId);
        }
    }
}