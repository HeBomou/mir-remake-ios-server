using System.Collections.Generic;
using MirRemakeBackend.Entity;

namespace MirRemakeBackend.GameLogic {
    class UnitInitializer {
        static class NetworkIdManager {
            static private HashSet<int> m_unitNetIdSet = new HashSet<int> ();
            static private int m_unitCnt = 0;
            static public int AssignNetworkId () {
                // 分配NetworkId
                while (true) {
                    ++m_unitCnt;
                    if (!m_unitNetIdSet.Contains (m_unitCnt))
                        break;
                }
                m_unitNetIdSet.Add (m_unitCnt);
                return m_unitCnt;
            }
            static public int[] AssignNetworkId (int num) {
                int[] res = new int[num];
                for (int i = 0; i < num; i++)
                    res[i] = AssignNetworkId ();
                return res;
            }
            static public void RemoveNetworkId (int netId) {
                m_unitNetIdSet.Remove (netId);
            }
        }
        public static UnitInitializer s_instance;
        public UnitInitializer () { InitAllMonster (); }
        private void InitAllMonster () {
            int monNum = EM_Unit.s_instance.GetMonsterNum ();
            int[] netIdArr = NetworkIdManager.AssignNetworkId (monNum);

            var mons = GL_UnitBattleAttribute.s_instance.NotifyInitAllMonster (netIdArr);
            GL_Sight.s_instance.NotifyInitAllMonster (mons);
        }
        public int AssignNetworkId () {
            return NetworkIdManager.AssignNetworkId ();
        }
        public void CommandInitCharacterId (int netId, int charId) {
            // 角色
            var newChar = GL_UnitBattleAttribute.s_instance.NotifyInitCharacter (netId, charId);

            // Sight
            GL_Sight.s_instance.NotifyInitCharacter (newChar);

            // 道具
            GL_CharacterItem.s_instance.NotifyInitCharacter (netId, charId);

            // 技能
            GL_Skill.s_instance.NotifyInitCharacter (netId, charId);

            // 任务
            GL_Mission.s_instance.NotifyInitCharacter (netId, charId);
        }
        public void CommandRemoveCharacter (int netId) {
            GL_UnitBattleAttribute.s_instance.NotifyRemoveCharacter (netId);
            GL_Sight.s_instance.NotifyRemoveCharacter (netId);
            GL_CharacterItem.s_instance.NotifyRemoveCharacter (netId);
            GL_Skill.s_instance.NotifyRemoveCharacter (netId);
            GL_Mission.s_instance.NotifyRemoveCharacter (netId);
            // 释放NetId
            NetworkIdManager.RemoveNetworkId (netId);
        }
    }
}