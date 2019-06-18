using System.Collections.Generic;
using MirRemakeBackend.Data;

namespace MirRemakeBackend.DataEntity {
    /// <summary>
    /// 数据型Entity的容器  
    /// 技能  
    /// </summary>
    class DEM_Mission {
        private Dictionary<short, DE_Mission> m_missionDict = new Dictionary<short, DE_Mission> ();
        public DEM_Mission (IDS_Mission ds) {
            var doArr = ds.GetAllMission ();
            foreach (var mDo in doArr) {
                var de = new DE_Mission (mDo);
                m_missionDict.Add (mDo.m_id, de);
            }
        }
        public DE_Mission GetMissionById (short missionId) {
            DE_Mission res = null;
            m_missionDict.TryGetValue (missionId, out res);
            return res;
        }
    }
}