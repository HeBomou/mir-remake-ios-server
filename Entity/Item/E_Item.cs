using System;
using System.Collections.Generic;
using MirRemakeBackend.DataEntity;
using MirRemakeBackend.DynamicData;

namespace MirRemakeBackend.Entity {
    abstract class E_Item {
        public static E_Item s_emptyItem = new E_EmptyItem ();
        public long m_realId;
        public short m_itemId;
        public DE_Item m_itemDe;
        public virtual ItemType m_Type { get; }
        public short m_num;
        public bool m_IsEmpty { get { return m_Type == ItemType.EMPTY; } }
        public short m_MaxNum { get { return m_itemDe.m_maxNum; } }
        protected virtual void Reset (DE_Item de, DDO_Item ddo) {
            m_itemDe = de;
            m_itemId = ddo.m_itemId;
            m_realId = ddo.m_realId;
            m_num = ddo.m_num;
        }
        /// <summary>
        /// 从一个道具中移除一定的数量  
        /// </summary>
        /// <returns>整格用完返回true</returns>
        public bool RemoveNum (short num) {
            m_num = (short)Math.Max (0, m_num - num);
            return m_num == 0;
        }
    }
}