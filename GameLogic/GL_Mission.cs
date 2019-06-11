using System.Collections.Generic;
using MirRemakeBackend.DynamicData;
using MirRemakeBackend.Entity;
using MirRemakeBackend.Network;

namespace MirRemakeBackend.GameLogic {
    partial class GL_Mission : GameLogicBase {
        public static GL_Mission s_instance;
        private IDDS_Mission m_misDds;
        public GL_Mission (IDDS_Mission mDds, INetworkService ns) : base (ns) {
            m_misDds = mDds;
        }
        public override void Tick (float dT) { }
        public override void NetworkTick () { }
        public void CommandApplyTalkToNpc (int netId, short npcId, short missionId) {
            // TODO:
        }
        public void CommandApplyAcceptMission (int netId, short misId) {
            var charObj = EM_Unit.s_instance.GetCharacterByNetworkId (netId);
            if (charObj == null) return;
            // 实例化
            var mis = EM_Mission.s_instance.AcceptMission (netId, misId);
            if (mis == null) return;
            // 数据与client
            m_misDds.UpdateMission (mis.GetDdo (charObj.m_characterId));
            m_networkService.SendServerCommand (SC_ApplySelfAcceptMission.Instance (new List<int> (netId), misId));
            // TODO: 处理任务条件监听
        }
        public void CommandApplyDeliveryMission (int netId, short misId) {
            var charObj = EM_Unit.s_instance.GetCharacterByNetworkId (netId);
            if (charObj == null) return;
            var misObj = EM_Mission.s_instance.GetAcceptedMission (netId, misId);
            if (misObj == null) return;
            if (!misObj.m_IsFinished)
                return;
            // 移除实例
            List<short> acableMis, unaMis;
            EM_Mission.s_instance.DeliveryMission (netId, misObj, charObj.m_Occupation, charObj.m_Level, out acableMis, out unaMis);
            // dds 与 client
            m_misDds.DeleteMission (misId, charObj.m_characterId);
            for (int i=0; i<acableMis.Count; i++)
                m_misDds.InsertMission (new DDO_Mission (acableMis[i], charObj.m_characterId, false, new List<int> ()));
            for (int i=0; i<unaMis.Count; i++)
                m_misDds.InsertMission (new DDO_Mission (unaMis[i], charObj.m_characterId, false, new List<int> ()));
            m_networkService.SendServerCommand (SC_ApplySelfDeliverMission.Instance (netId, misId));
            m_networkService.SendServerCommand (SC_ApplySelfMissionUnlock.Instance (netId, acableMis, unaMis));
            // TODO: 移除监听
            // 其他模块
            GL_Property.s_instance.NotifyUpdateCurrency (charObj, CurrencyType.VIRTUAL, misObj.m_BonusVirtualCurrency);
            GL_Property.s_instance.NotifyGainItem (charObj, misObj.m_BonusItemIdAndNumList);
            GL_CharacterLevel.s_instance.NotifyGainExperience (charObj, misObj.m_BonusExperience);
        }
        public void CommandCancelMission (int netId, short misId) {
            var charId = EM_Unit.s_instance.GetCharIdByNetworkId (netId);
            if (charId == -1) return;
            var misObj = EM_Mission.s_instance.GetAcceptedMission (netId, misId);
            if (misObj == null) return;
            // 移除实例 数据 client
            EM_Mission.s_instance.CancelMission (netId, misObj);
            m_misDds.UpdateMission (misObj.GetDdo (charId));
            m_networkService.SendServerCommand (SC_ApplySelfCancelMission.Instance (netId, misId));
            // TODO: 移除监听
        }
    }
}