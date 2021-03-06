using System.Collections.Generic;
using LiteNetLib.Utils;
using MirRemakeBackend.GameLogic;

namespace MirRemakeBackend.Network {
    interface IClientCommand {
        NetworkToServerDataType m_DataType { get; }
        void Execute (NetDataReader reader, int netId);
    }

    class CC_InitRegister : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.INIT_REGISTER; } }
        public void Execute (NetDataReader reader, int netId) {
            string username = reader.GetString ();
            string pwd = reader.GetString ();
            string pwdProtectProblem = reader.GetString ();
            string pwdProtectAnswer = reader.GetString ();;
            GL_User.s_instance.CommandRegister (netId, username, pwd, pwdProtectProblem, pwdProtectAnswer);
        }
    }

    class CC_InitLogin : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.INIT_LOGIN; } }
        public void Execute (NetDataReader reader, int netId) {
            string username = reader.GetString ();
            string pwd = reader.GetString ();
            GL_User.s_instance.CommandLogin (netId, username, pwd);
        }
    }

    class CC_InitModifyPassword : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.INIT_MODIFY_PASSWORD; } }
        public void Execute (NetDataReader reader, int netId) {
            string username = reader.GetString ();
            string oldPwd = reader.GetString ();
            string newPwd = reader.GetString ();
            GL_User.s_instance.CommandModifyPassword (netId, username, oldPwd, newPwd);
        }
    }

    class CC_InitGetPasswordProtectionProblem : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.INIT_GET_PASSWORD_PROTECT_PROBLEM; } }
        public void Execute (NetDataReader reader, int netId) {
            string username = reader.GetString ();
            GL_User.s_instance.CommandGetPwdProtectProblem (netId, username);
        }
    }

    class CC_InitFindPassword : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.INIT_FIND_PASSWORD; } }
        public void Execute (NetDataReader reader, int netId) {
            string username = reader.GetString ();
            string pwdProtectAnswer = reader.GetString ();
            string newPwd = reader.GetString ();
            GL_User.s_instance.CommandFindPassword (netId, username, pwdProtectAnswer, newPwd);
        }
    }

    class CC_InitCreateCharacter : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.INIT_CREATE_CHARACTER; } }
        public void Execute (NetDataReader reader, int netId) {
            int playerId = reader.GetInt ();
            OccupationType ocp = (OccupationType) reader.GetByte ();
            string name = reader.GetString ();
            GL_User.s_instance.CommandCreateCharacter (netId, playerId, ocp, name);
        }
    }

    /// <summary>
    /// 初始传入CharacterId
    /// </summary>
    class CC_InitCharacterId : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.INIT_CHARACTER_ID; } }
        public void Execute (NetDataReader reader, int netId) {
            int charId = reader.GetInt ();
            GL_CharacterInit.s_instance.CommandInitCharacterId (netId, charId);
        }
    }

    /// <summary>
    /// 同步位置
    /// </summary>
    class CC_SetPosition : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.SET_POSITION; } }
        public void Execute (NetDataReader reader, int netId) {
            var pos = reader.GetVector2 ();
            GL_CharacterAction.s_instance.CommandSetPosition (netId, pos);
        }
    }

    /// <summary>
    /// 更换快捷键
    /// </summary>
    class CC_ApplyChangeShortcut : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_CHANGE_SHORTCUT; } }
        public void Execute (NetDataReader reader, int netId) {
            NO_Shortcut shortcut = reader.GetShortcut ();
            GL_Shortcut.s_instance.CommandUpdateShortcut (netId, shortcut);
        }
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    class CC_ApplyCastSkillBegin : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_CAST_SKILL_BEGIN; } }
        public void Execute (NetDataReader reader, int netId) {
            short skillId = reader.GetShort ();
            NO_SkillParam skillParm = reader.GetSkillParam ();
            byte cnt = reader.GetByte ();
            int[] hitTargetArr = new int[cnt];
            for (int i = 0; i < cnt; i++) {
                hitTargetArr[i] = reader.GetInt ();
            }
            GL_CharacterAction.s_instance.CommandApplyCastSkillBegin (netId, skillId, skillParm, hitTargetArr);
        }
    }

    /// <summary>
    /// 属性点分配
    /// </summary>
    class CC_ApplyDistributePoints : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_DISTRIBUTE_POINTS; } }
        public void Execute (NetDataReader reader, int netId) {
            short strength = reader.GetShort ();
            short intelligence = reader.GetShort ();
            short agility = reader.GetShort ();
            short spirit = reader.GetShort ();
            GL_CharacterAttribute.s_instance.CommandApplyDistributePoints (netId, strength, intelligence, agility, spirit);
        }
    }

    /// <summary>
    /// 属性点分配
    /// </summary>
    class CC_ApplyShowCharacterAttribute : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_SHOW_CHARACTER_ATTRIBUTE; } }
        public void Execute (NetDataReader reader, int netId) {
            int tarCharId = reader.GetInt ();
            GL_CharacterAttribute.s_instance.CommandRequireCharacterAttribute (netId, tarCharId);
        }
    }

    /// <summary>
    /// 打开战力排行榜时请求刷新操作
    /// </summary>
    class CC_ApplyRefreshFightCapacityRank : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_REFRESH_FIGHT_CAPACITY_RANK; } }
        public void Execute (NetDataReader reader, int netId) {
            OccupationType ocp = (OccupationType) reader.GetByte ();
            GL_CharacterCombatEfct.s_instance.CommandGetCombatEffectivenessRank (netId, ocp);
        }
    }

    /// <summary>
    /// 技能升级
    /// </summary>
    class CC_ApplyUpdateSkillLevel : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_UPDATE_SKILL_LEVEL; } }
        public void Execute (NetDataReader reader, int netId) {
            short skillId = reader.GetShort ();
            short targetSkillLevel = reader.GetShort ();
            GL_Skill.s_instance.CommandUpdateSkillLevel (netId, skillId, targetSkillLevel);
        }
    }

    /// <summary>
    /// 地面物品拾取
    /// </summary>
    class CC_ApplyPickUpItemOnGround : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_PICK_UP_ITEM_ON_GROUND; } }
        public void Execute (NetDataReader reader, int netId) {
            long gndItemId = reader.GetLong ();
            GL_Item.s_instance.CommandPickUpGroundItem (netId, gndItemId);
        }
    }

    /// <summary>
    /// 丢弃物品
    /// </summary>
    class CC_ApplyDropItemOntoGround : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_DROP_ITEM_ONTO_GROUND; } }
        public void Execute (NetDataReader reader, int netId) {
            long realId = reader.GetLong ();
            short num = reader.GetShort ();
            GL_Item.s_instance.CommandDropItemOntoGround (netId, realId, num);
        }
    }

    /// <summary>
    /// 在仓库中储存物品
    /// </summary>
    class CC_ApplySaveItemIntoStoreHouse : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_SAVE_ITEM_INTO_STORE_HOUSE; } }
        public void Execute (NetDataReader reader, int netId) {
            long realId = reader.GetLong ();
            short num = reader.GetShort ();
            // TODO: 存储物品
        }
    }

    /// <summary>
    /// 从仓库中取出物品
    /// </summary>
    class CC_ApplyTakeOutItemFromStoreHouse : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_TAKE_OUT_ITEM_FROM_STORE_HOUSE; } }
        public void Execute (NetDataReader reader, int netId) {
            long realId = reader.GetLong ();
            short num = reader.GetShort ();
            // TODO: 取出物品
        }
    }

    /// <summary>
    /// 使用消耗品
    /// </summary>
    class CC_ApplyUseConsumableItem : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_USE_CONSUMABLE_ITEM; } }
        public void Execute (NetDataReader reader, int netId) {
            long realId = reader.GetLong ();
            GL_Item.s_instance.CommandApplyUseConsumableItem (netId, realId);
        }
    }

    /// <summary>
    /// 使用背包中的装备
    /// </summary>
    class CC_ApplyUseEquipmentItem : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_USE_EQUIPMENT_ITEM; } }
        public void Execute (NetDataReader reader, int netId) {
            long realId = reader.GetLong ();
            GL_Item.s_instance.CommandApplyUseEquipmentItem (netId, realId);
        }
    }

    /// <summary>
    /// 卸除的装备
    /// </summary>
    class CC_ApplyTakeOffEquipmentItem : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_TAKE_OFF_EQUIPMENT_ITEM; } }
        public void Execute (NetDataReader reader, int netId) {
            long realId = reader.GetLong ();
            GL_Item.s_instance.CommandApplyTakeOffEquipmentItem (netId, realId);
        }
    }

    /// <summary>
    /// 出售背包中的物品
    /// </summary>
    class CC_ApplySellItemInBag : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_SELL_ITEM_IN_BAG; } }
        public void Execute (NetDataReader reader, int netId) {
            long realId = reader.GetLong ();
            short num = reader.GetShort ();
            GL_Item.s_instance.CommandApplySellItemInBag (netId, realId, num);
        }
    }
    /// <summary>
    /// /// 购买物品放入背包
    /// </summary>
    class CC_ApplyBuyItemIntoBag : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_BUY_ITEM_INTO_BAG; } }
        public void Execute (NetDataReader reader, int netId) {
            short itemId = reader.GetShort ();
            short num = reader.GetShort ();
            GL_Item.s_instance.CommandApplyBuyItemIntoBag (netId, itemId, num);
        }
    }
    class CC_ApplyBuildEquipment : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_BUILD_EQUIPMENT; } }
        public void Execute (NetDataReader reader, int netId) {
            byte matNum = reader.GetByte ();
            var matArr = new (short, short) [matNum];
            for (int i = 0; i < matNum; i++) {
                short itemId = reader.GetShort ();
                short itemNum = reader.GetShort ();
                matArr[i] = (itemId, itemNum);
            }
            GL_Item.s_instance.CommandApplyBuildEquipment (netId, matArr);
        }
    }
    class CC_ApplyStrengthenEquipment : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_STRENGTHEN_EQUPMENT; } }
        public void Execute (NetDataReader reader, int netId) {
            long realId = reader.GetLong ();
            GL_Item.s_instance.CommandApplyStrengthenEquipment (netId, realId);
        }
    }
    class CC_ApplyEnchantEquipment : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_ENCHANT_EQUIPMENT; } }
        public void Execute (NetDataReader reader, int netId) {
            long equipmentRealId = reader.GetLong ();
            long enchantmentRealId = reader.GetLong ();
            GL_Item.s_instance.CommandApplyEnchantEquipment (netId, equipmentRealId, enchantmentRealId);
        }
    }
    class CC_ApplyInlayGemInEquipment : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_INLAY_GEM_IN_EQUIPMENT; } }
        public void Execute (NetDataReader reader, int netId) {
            long equipmentRealId = reader.GetLong ();
            long gemRealId = reader.GetLong ();
            GL_Item.s_instance.CommandApplyInlayGemInEquipment (netId, equipmentRealId, gemRealId);
        }
    }
    class CC_ApplyMakeHoleInEquipment : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_MAKE_HOLE_IN_EQUIPMENT; } }
        public void Execute (NetDataReader reader, int netId) {
            long equipmentRealId = reader.GetLong ();
            GL_Item.s_instance.CommandApplyMakeHoleInEquipment (netId, equipmentRealId);
        }
    }
    class CC_ApplyDisjointEquipment : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_DISJOINT_EQUIPMENT; } }
        public void Execute (NetDataReader reader, int netId) {
            long equipmentRealId = reader.GetLong ();
            GL_Item.s_instance.CommandApplyDisjointEquipment (netId, equipmentRealId);
        }
    }
    class CC_ApplyAutoDisjoint : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_AUTO_DISJOINT; } }
        public void Execute (NetDataReader reader, int netId) {
            byte itemQualities = reader.GetByte ();
            GL_Item.s_instance.CommandApplyAutoDisjointEquipment (netId, itemQualities);
        }
    }

    class CC_ApplyAutoPickOn : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_AUTO_PICK_ON; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_Item.s_instance.CommandApplyAutoPickUpOn (netId);
        }
    }

    class CC_ApplyAutoPickOff : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_AUTO_PICK_OFF; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_Item.s_instance.CommandApplyAutoPickUpOff (netId);
        }
    }

    class CC_ApplySetUpMarket : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_SET_UP_MARKET; } }
        public void Execute (NetDataReader reader, int netId) {
            short itemCnt = reader.GetShort ();
            List<NO_MarketItem> itemList = new List<NO_MarketItem> (itemCnt);
            for (int i = 0; i < itemCnt; i++)
                itemList.Add (reader.GetMarketItem ());
            GL_Item.s_instance.CommandApplyPSetUpMarket (netId, itemList);
        }
    }

    class CC_ApplyPackUpMarket : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_PACK_UP_MARKET; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_Item.s_instance.CommandApplyPackUpMarket (netId);
        }
    }

    class CC_ApplyEnterMarket : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_ENTER_MARKET; } }
        public void Execute (NetDataReader reader, int netId) {
            int holderNetId = reader.GetInt ();
            GL_Item.s_instance.CommandApplyEnterMarket (netId, holderNetId);
        }
    }

    class CC_ApplyBuyItemInMarket : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_BUY_ITEM_IN_MARKET; } }
        public void Execute (NetDataReader reader, int netId) {
            int holderNetId = reader.GetInt ();
            long realId = reader.GetLong ();
            short num = reader.GetShort ();
            CurrencyType cyType = (CurrencyType) reader.GetByte ();
            GL_Item.s_instance.CommandApplyBuyItemInMarket (netId, holderNetId, realId, num, cyType);
        }
    }

    /// <summary>
    /// 接受任务
    /// </summary>
    class CC_ApplyAcceptMission : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_ACCEPT_MISSION; } }
        public void Execute (NetDataReader reader, int netId) {
            short missionId = reader.GetShort ();
            GL_Mission.s_instance.CommandApplyAcceptMission (netId, missionId);
        }
    }

    /// <summary>
    /// 交付任务
    /// </summary>
    class CC_ApplyDeliverMission : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_DELIVER_MISSION; } }
        public void Execute (NetDataReader reader, int netId) {
            short missionId = reader.GetShort ();
            GL_Mission.s_instance.CommandApplyDeliveryMission (netId, missionId);
        }
    }

    /// <summary>
    /// 放弃任务
    /// </summary>
    class CC_ApplyCancelMission : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_CANCEL_MISSION; } }
        public void Execute (NetDataReader reader, int netId) {
            short missionId = reader.GetShort ();
            GL_Mission.s_instance.CommandCancelMission (netId, missionId);
        }
    }

    /// <summary>
    /// 与任务Npc交流
    /// </summary>
    class CC_ApplyTalkToMissionNpc : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_TALK_TO_MISSION_NPC; } }
        public void Execute (NetDataReader reader, int netId) {
            short misId = reader.GetShort ();
            short misTarId = reader.GetShort ();
            GL_Mission.s_instance.CommandApplyTalkToNpc (netId, misId, misTarId);
        }
    }

    /// <summary>
    /// 装配一个称号
    /// </summary>
    class CC_ApplyAttachTitle : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_ATTACH_TITLE; } }
        public void Execute (NetDataReader reader, int netId) {
            short misId = reader.GetShort ();
            GL_Mission.s_instance.CommandApplyAttachTitle (netId, misId);
        }
    }

    /// <summary>
    /// 卸除当前称号
    /// </summary>
    class CC_ApplyDetachTitle : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_DETACH_TITLE; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_Mission.s_instance.CommandApplyDetachTitle (netId);
        }
    }

    /// <summary>
    /// 获取商场 常规商品列表
    /// </summary>
    class CC_ApplyShowMallNormal : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_SHOW_MALL; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_Mall.s_instance.CommandRequireMall (netId);
        }
    }

    /// <summary>
    /// 商城物品购买
    /// </summary>
    class CC_ApplyBuyItemInMall : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_BUY_ITEM_IN_MALL; } }
        public void Execute (NetDataReader reader, int netId) {
            int mallItemId = reader.GetInt ();
            short num = reader.GetShort ();
            CurrencyType cyType = (CurrencyType) reader.GetByte ();
            GL_Mall.s_instance.CommandBuyItemInMall (netId, mallItemId, num, cyType);
        }
    }

    /// <summary>
    /// 发送消息  
    /// </summary>
    class CC_ApplySendMessage : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_SEND_MESSAGE; } }
        public void Execute (NetDataReader reader, int netId) {
            ChattingChanelType channel = (ChattingChanelType) reader.GetByte ();
            string msg = reader.GetString ();
            int toNetId = reader.GetInt ();
            GL_Chat.s_instance.CommandSendMessage (netId, channel, msg, toNetId);
        }
    }

    class CC_ApplyRespawnHome : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_RESPAWN_HOME; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_CharacterAction.s_instance.CommandApplyRespawnHome (netId);
        }
    }

    class CC_ApplyRespawnPlace : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_RESPAWN_PLACE; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_CharacterAction.s_instance.CommandApplyRespawnPlace (netId);
        }
    }

    class CC_ApplyShowMailBox : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_SHOW_MAIL_BOX; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_Mail.s_instance.CommandApplyShowMailBox (netId);
        }
    }

    class CC_ApplyReadMail : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_READ_MAIL; } }
        public void Execute (NetDataReader reader, int netId) {
            int mailId = reader.GetInt ();
            GL_Mail.s_instance.CommandApplyReadMail (netId, mailId);
        }
    }

    class CC_ApplyReadAllMail : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_READ_ALL_MAIL; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_Mail.s_instance.CommandApplyReadAllMail (netId);
        }
    }

    class CC_ApplyReceiveMail : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_RECEIVE_MAIL; } }
        public void Execute (NetDataReader reader, int netId) {
            int mailId = reader.GetInt ();
            GL_Mail.s_instance.CommandApplyReceiveMail (netId, mailId);
        }
    }

    class CC_ApplyReceiveAllMail : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_RECEIVE_ALL_MAIL; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_Mail.s_instance.CommandApplyReceiveAllMail (netId);
        }
    }

    class CC_ApplyCreateAlliance : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_CREATE_ALLIANCE; } }
        public void Execute (NetDataReader reader, int netId) {
            string name = reader.GetString ();
            GL_Alliance.s_instance.CommandCreateAlliance (netId, name);
        }
    }

    class CC_ApplyDissolveAlliance : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_DISSOLVE_ALLIANCE; } }
        public void Execute (NetDataReader reader, int netId) {
            int allianceId = reader.GetInt ();
            GL_Alliance.s_instance.CommandDissolveAlliance (netId, allianceId);
        }
    }

    class CC_ApplyTransferAlliance : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_TRANSFER_ALLIANCE; } }
        public void Execute (NetDataReader reader, int netId) {
            int allianceId = reader.GetInt ();
            int tarCharId = reader.GetInt ();
            GL_Alliance.s_instance.CommandTransferAlliance (netId, allianceId, tarCharId);
        }
    }

    class CC_ApplyApplyToJoinAlliance : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_APPLY_TO_JOIN_ALLIANCE; } }
        public void Execute (NetDataReader reader, int netId) {
            int allianceId = reader.GetInt ();
            GL_Alliance.s_instance.CommandApplyToJoin (netId, allianceId);
        }
    }

    class CC_ApplyRefuseToJoinAlliance : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_REFUSE_TO_JOIN_ALLIANCE; } }
        public void Execute (NetDataReader reader, int netId) {
            int applyId = reader.GetInt ();
            GL_Alliance.s_instance.CommandRefuseToJoin (netId, applyId);
        }
    }

    class CC_ApplyApproveToJoinAlliance : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_APPROVE_TO_JOIN_ALLIANCE; } }
        public void Execute (NetDataReader reader, int netId) {
            int applyId = reader.GetInt ();
            GL_Alliance.s_instance.CommandApproveToJoin (netId, applyId);
        }
    }

    class CC_ApplyChangeAllianceJob : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_CHANGE_ALLIANCE_JOB; } }
        public void Execute (NetDataReader reader, int netId) {
            int tarCharId = reader.GetInt ();
            AllianceJob job = (AllianceJob) reader.GetByte ();
            GL_Alliance.s_instance.CommandChangeJob (netId, tarCharId, job);
        }
    }

    class CC_ConsoleGainCyByName : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.CONSOLE_GAIN_CY_BY_NAME; } }
        public void Execute (NetDataReader reader, int netId) {
            string name = reader.GetString ();
            CurrencyType cyType = (CurrencyType) reader.GetByte ();
            long cy = reader.GetLong ();
            GL_Console.s_instance.CommandGainCurrencyByName (netId, name, cyType, cy);
        }
    }

    class CC_ConsoleSealCharacterByName : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.CONSOLE_SEAL_CHARACTER_BY_NAME; } }
        public void Execute (NetDataReader reader, int netId) {
            string name = reader.GetString ();
            // TODO: 根据角色名封号
        }
    }

    class CC_ConsoleReleaseNotice : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.CONSOLE_RELEASE_NOTICE; } }
        public void Execute (NetDataReader reader, int netId) {
            string title = reader.GetString ();
            string detail = reader.GetString ();
            GL_Console.s_instance.CommandReleaseNotice (netId, title, detail);
        }
    }

    class CC_ConsoleChargeMoney : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.CONSOLE_CHARGE_MONEY; } }
        public void Execute (NetDataReader reader, int netId) {
            string charName = reader.GetString ();
            int money = reader.GetInt ();
            GL_Console.s_instance.CommandChargeMoney (netId, charName, money);
        }
    }

    class CC_ApplyShowNotice : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_SHOW_NOTICE; } }
        public void Execute (NetDataReader reader, int netId) {
            GL_Notice.s_instance.CommandShowNotice (netId);
        }
    }

    class CC_ApplyDeleteNotice : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.APPLY_DELETE_NOTICE; } }
        public void Execute (NetDataReader reader, int netId) {
            int noticeId = reader.GetInt ();
            GL_Console.s_instance.CommandDeleteNotice (netId, noticeId);
        }
    }

    class CC_TestGainCy : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.TEST_GAIN_CY; } }
        public void Execute (NetDataReader reader, int netId) {
            CurrencyType cyType = (CurrencyType) reader.GetByte ();
            long cy = reader.GetLong ();
            GL_Wallet.s_instance.CommandGainCurrency (netId, cyType, cy);
        }
    }

    class CC_TestGainExp : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.TEST_GAIN_EXP; } }
        public void Execute (NetDataReader reader, int netId) {
            int exp = reader.GetInt ();
            GL_CharacterAttribute.s_instance.CommandGainExperience (netId, exp);
        }
    }

    class CC_TestGainMasterly : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.TEST_GAIN_MASTERLY; } }
        public void Execute (NetDataReader reader, int netId) {
            short skId = reader.GetShort ();
            int masterly = reader.GetInt ();
            GL_Skill.s_instance.CommandGainMasterly (netId, skId, masterly);
        }
    }

    class CC_TestGainItem : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.TEST_GAIN_ITEM; } }
        public void Execute (NetDataReader reader, int netId) {
            short itemId = reader.GetShort ();
            short num = reader.GetShort ();
            GL_Item.s_instance.CommandTestGainItem (netId, itemId, num);
        }
    }

    class CC_TestSendMailToAll : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.TEST_SEND_MAIL_TO_ALL; } }
        public void Execute (NetDataReader reader, int netId) {
            string senderName = reader.GetString ();
            string title = reader.GetString ();
            string detail = reader.GetString ();
            byte itemCnt = reader.GetByte ();
            (short, short) [] itemIdAndNumArr = new (short, short) [itemCnt];
            for (int i = 0; i < itemCnt; i++)
                itemIdAndNumArr[i] = (reader.GetShort (), reader.GetShort ());
            long vCy = reader.GetLong ();
            long cCy = reader.GetLong ();
            GL_Mail.s_instance.CommandTestSendMailToAll (senderName, title, detail, itemIdAndNumArr, vCy, cCy);
        }
    }

    class CC_TestSendMailToAllOnline : IClientCommand {
        public NetworkToServerDataType m_DataType { get { return NetworkToServerDataType.TEST_SEND_MAIL_TO_ALL_ONLINE; } }
        public void Execute (NetDataReader reader, int netId) {
            string senderName = reader.GetString ();
            string title = reader.GetString ();
            string detail = reader.GetString ();
            byte itemCnt = reader.GetByte ();
            (short, short) [] itemIdAndNumArr = new (short, short) [itemCnt];
            for (int i = 0; i < itemCnt; i++)
                itemIdAndNumArr[i] = (reader.GetShort (), reader.GetShort ());
            long vCy = reader.GetLong ();
            long cCy = reader.GetLong ();
            GL_Mail.s_instance.CommandTestSendMailToAllOnline (senderName, title, detail, itemIdAndNumArr, vCy, cCy);
        }
    }
}