using System.Collections.Generic;
using MirRemakeBackend.Entity;
using MirRemakeBackend.Network;

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
            var mons = EM_Unit.s_instance.InitAllMonster (netIdArr);
            EM_Sight.s_instance.InitAllMonster (mons);
            EM_Status.s_instance.InitAllMonster (netIdArr);
        }
        public int AssignNetworkId () {
            return NetworkIdManager.AssignNetworkId ();
        }
        public void CommandInitCharacterId (int netId, int charId) {
            // TODO: 应当完全依赖相应GL来添加角色
            // 实例化角色
            E_Character newChar = EM_Unit.s_instance.InitCharacter (netId, charId, m_charDds.GetCharacterById (charId));
            EM_Sight.s_instance.InitCharacter (newChar);
            // client 角色
            m_netService.SendServerCommand (SC_InitSelfAttribute.Instance (
                netId,
                newChar.m_Occupation,
                newChar.m_Level,
                newChar.m_experience,
                newChar.m_Strength,
                newChar.m_Intelligence,
                newChar.m_Agility,
                newChar.m_Spirit,
                newChar.m_TotalMainPoint));

            // 实例化道具
            var bagDdo = m_itemDds.GetBagByCharacterId (charId);
            var storeHouseDdo = m_itemDds.GetStoreHouseByCharacterId (charId);
            var eqRegionDdo = m_itemDds.GetEquipmentRegionByCharacterId (charId);
            var equipmentDdo = m_itemDds.GetAllEquipmentByCharacterId (charId);
            E_Repository bag, storeHouse;
            E_EquipmentRegion eqRegion;
            EM_Item.s_instance.InitCharacter (netId, bagDdo, storeHouseDdo, eqRegionDdo, equipmentDdo, out bag, out storeHouse, out eqRegion);
            // client
            m_netService.SendServerCommand (SC_InitSelfItem.Instance (new List<int> () { netId }, bag.GetNo (), storeHouse.GetNo (), eqRegion.GetNo (), newChar.m_VirtualCurrency, newChar.m_ChargeCurrency));

            // 实例化技能
            var skillDdoList = m_skillDds.GetSkillListByCharacterId (charId);
            E_Skill[] skillArr = EM_Skill.s_instance.InitCharacter (netId, charId, skillDdoList);
            // client
            var skillIdAndLvAndMasterlyArr = new (short, short, int) [skillArr.Length];
            for (int i = 0; i < skillArr.Length; i++)
                skillIdAndLvAndMasterlyArr[i] = (skillArr[i].m_SkillId, skillArr[i].m_skillLevel, skillArr[i].m_masterly);
            m_netService.SendServerCommand (SC_InitSelfSkill.Instance (netId, skillIdAndLvAndMasterlyArr));

            // 初始化状态
            EM_Status.s_instance.InitCharacterStatus (netId);

            // 初始化任务
            GL_Mission.s_instance.NotifyInitCharacter (newChar.m_networkId, newChar.m_characterId);
        }
        public void CommandRemoveCharacter (int netId) {
            var charObj = EM_Unit.s_instance.GetCharacterByNetworkId (netId);
            if (charObj == null) return;

            GL_Mission.s_instance.NotifyRemoveCharacter (charObj);
            GL_Item.s_instance.NotifyRemoveCharacter (charObj);
            GL_Sight.s_instance.NotifyRemoveCharacter (charObj);
            GL_Skill.s_instance.NotifyRemoveCharacter (charObj);
            // TODO: 应当完全依赖相应GL来移除
            EM_Status.s_instance.RemoveCharacterStatus (netId);
            EM_Unit.s_instance.RemoveCharacter (netId);
            NetworkIdManager.RemoveNetworkId (netId);
        }
    }
}