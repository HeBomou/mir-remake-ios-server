using System.Collections.Generic;
using System.Numerics;
using MirRemakeBackend.DataEntity;
using MirRemakeBackend.DynamicData;
using MirRemakeBackend.Util;

namespace MirRemakeBackend.Entity {
    /// <summary>
    /// 索引场景中所有的Character  
    /// </summary>
    class EM_Character {
        public static EM_Character s_instance;
        private DEM_Character m_dem;
        private IDDS_Character m_charDds;
        private IDDS_CharacterAttribute m_charAttrDds;
        private const int c_characterPoolSize = 400;
        public ObjectPool<E_Character> m_characterPool = new ObjectPool<E_Character> (c_characterPoolSize);
        private Dictionary<int, E_Character> m_networkIdAndCharacterDict = new Dictionary<int, E_Character> ();
        private Dictionary<int, int> m_netIdAndCharIdDict = new Dictionary<int, int> ();
        private Dictionary<int, int> m_charIdAndNetIdDict = new Dictionary<int, int> ();
        public EM_Character (DEM_Character dem, IDDS_Character charDds, IDDS_CharacterAttribute charAttrDds) {
            m_dem = dem;
            m_charDds = charDds;
            m_charAttrDds = charAttrDds;
        }
        public int CreateCharacter (int playerId, OccupationType ocp, string name) {
            return m_charDds.InsertCharacter (new DDO_Character (-1, playerId, ocp, name));
        }
        public void CreateCharacterAttribute (int charId) {
            m_charAttrDds.InsertCharacterAttribute (new DDO_CharacterAttribute (charId, 1, 0, 0, 0, 0, 0));
        }
        /// <summary>
        /// 从数据库读取角色信息  
        /// 并在场景中索引新接入的角色  
        /// 若场景中已存在该角色, 则直接返回  
        /// </summary>
        public E_Character InitCharacter (int netId, int charId) {
            E_Character newChar = null;
            if (m_networkIdAndCharacterDict.TryGetValue (netId, out newChar))
                return newChar;
            // 持久层获取
            DDO_Character charDdo;
            DDO_CharacterAttribute charAttrDdo;
            if (!m_charDds.GetCharacterById (charId, out charDdo) ||
                !m_charAttrDds.GetCharacterAttributeByCharacterId (charId, out charAttrDdo)
                // !m_charPosDds.GetCharacterPosition (charId, out charPosDdo)
            )
                return null;
            newChar = m_characterPool.GetInstance ();
            DE_Character charDe;
            DE_Unit unitDe;
            DE_CharacterData charDataDe;
            m_dem.GetCharacterByOccupationAndLevel (charDdo.m_occupation, charAttrDdo.m_level, out charDe, out unitDe, out charDataDe);

            m_netIdAndCharIdDict[netId] = charId;
            m_charIdAndNetIdDict[charId] = netId;
            m_networkIdAndCharacterDict[netId] = newChar;
            newChar.Reset (netId, charDe, unitDe, charDataDe, charDdo, charAttrDdo, new Vector2 (165, 116 /* 42, 24 */ ));
            return newChar;
        }
        /// <summary>
        /// 从场景中移除角色  
        /// </summary>
        /// <param name="netId"></param>
        public void RemoveCharacter (int netId) {
            E_Character charObj = null;
            if (!m_networkIdAndCharacterDict.TryGetValue (netId, out charObj))
                return;
            m_netIdAndCharIdDict.Remove (netId);
            m_charIdAndNetIdDict.Remove (charObj.m_characterId);
            m_networkIdAndCharacterDict.Remove (netId);
            m_characterPool.RecycleInstance (charObj);
        }
        public int[] GetAllCharId () {
            return m_charDds.GetAllCharacterId ();
        }
        public int GetCharIdByName (string name) {
            return m_charDds.GetCharacterIdByName (name);
        }
        public E_Character GetCharacterByNetworkId (int netId) {
            E_Character res = null;
            m_networkIdAndCharacterDict.TryGetValue (netId, out res);
            return res;
        }
        public int GetCharIdByNetId (int netId) {
            int charId;
            if (m_netIdAndCharIdDict.TryGetValue (netId, out charId))
                return charId;
            return -1;
        }
        public int GetNetIdByCharId (int charId) {
            int netId;
            if (m_charIdAndNetIdDict.TryGetValue (charId, out netId))
                return netId;
            return -1;
        }
        public Dictionary<int, E_Character>.Enumerator GetCharacterEnumerator () {
            return m_networkIdAndCharacterDict.GetEnumerator ();
        }
        public IReadOnlyList<DDO_Character> GetAllCharacterByPlayerId (int playerId) {
            return m_charDds.GetCharacterByPlayerId (playerId);
        }
        public short GetLevelByCharacterId (int charId) {
            DDO_CharacterAttribute ddo;
            if (m_charAttrDds.GetCharacterAttributeByCharacterId (charId, out ddo))
                return ddo.m_level;
            return -1;
        }
        public void SaveCharacterAttribute (E_Character charObj) {
            m_charAttrDds.UpdateCharacterAttribute (charObj.GetAttrDdo ());
        }
    }
}