using System;
using System.Collections.Generic;
using MirRemakeBackend.DynamicData;
using MirRemakeBackend.Network;

namespace MirRemakeBackend.Entity {
    class E_Mail {
        public int m_id;
        public int m_senderCharId;
        public string m_senderName;
        public int m_receiverCharId;
        public DateTime m_sendTime;
        public string m_title;
        public string m_detail;
        public List < (short, short) > m_itemIdAndNumList;
        public bool m_isRead;
        public bool m_isReceived;
        public void Reset (DDO_Mail ddo) {
            m_id = ddo.m_id;
            m_senderCharId = ddo.m_senderCharId;
            m_receiverCharId = ddo.m_receiverCharId;
            m_sendTime = ddo.m_sendTime;
            m_title = ddo.m_title;
            m_detail = ddo.m_detail;
            m_itemIdAndNumList = new List < (short, short) > (ddo.m_itemIdAndNumArr);
            m_isRead = ddo.m_isRead;
            m_isReceived = ddo.m_isReceived;
        }
        public void Reset (int id, int senderCharId, int recvCharId, DateTime sendTime, string title, string detail, List < (short, short) > itemIdAndNumList, bool isRead, bool isReceived) {
            m_id = id;
            m_senderCharId = senderCharId;
            m_receiverCharId = recvCharId;
            m_sendTime = sendTime;
            m_title = title;
            m_detail = detail;
            m_itemIdAndNumList = itemIdAndNumList;
            m_isRead = isRead;
            m_isReceived = isReceived;
        }
        public DDO_Mail GetDdo () {
            return new DDO_Mail (m_id, m_senderCharId, m_senderName, m_receiverCharId, m_sendTime, m_title, m_detail, m_itemIdAndNumList.ToArray (), m_isRead, m_isReceived);
        }
        public NO_Mail GetNo () {
            return new NO_Mail (m_id, m_senderCharId, m_senderName, m_receiverCharId, m_sendTime, m_title, m_detail, m_itemIdAndNumList, m_isRead, m_isReceived);
        }
    }
}