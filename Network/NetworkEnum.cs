namespace MirRemakeBackend.Network {
    enum NetworkToClientDataType : byte {
        // 初始化相关
        INIT_SELF_NETWORK_ID,
        INIT_SELF_ATTRIBUTE,
        INIT_SELF_SKILL,
        INIT_SELF_MISSION,
        INIT_SELF_ITEM,
        // 视野相关
        APPLY_OTHER_MONSTER_IN_SIGHT,
        APPLY_OTHER_CHARACTER_IN_SIGHT,
        APPLY_OTHER_ACTOR_UNIT_OUT_OF_SIGHT,
        // 位置 装备外观 属性
        SET_OTHER_POSITION,
        APPLY_ALL_CHANGE_EQUIPMENT,
        SET_ALL_HP_AND_MP,
        SET_SELF_CONCRETE_AND_SPECIAL_ATTRIBUTE,
        // 战斗状态与动画
        APPLY_OTHER_CAST_SKILL_BEGIN,
        APPLY_OTHER_CAST_SKILL_SING_CANCEL,
        APPLY_ALL_EFFECT,
        APPLY_ALL_STATUS,
        APPLY_ALL_DEAD,
        // 持有的物品
        APPLY_SELF_UPDATE_ITEM_NUM,
        APPLY_SELF_GAIN_ITEM,
        APPLY_SELF_MOVE_ITEM,
        // 地面物品
        APPLY_GROUND_ITEM_SHOW,
        APPLY_GROUND_ITEM_DISAPPEAR,
        // 任务相关
        APPLY_SELF_ACCECPT_MISSION,
        APPLY_SELF_DELIVER_MISSION,
        APPLY_SELF_CANCEL_MISSION,
        APPLY_SELF_SET_MISSION_PROGRESS,
        // 技能相关
        APPLY_SELF_UPDATE_SKILL_LEVEL_AND_MASTERLY,
        // 商城
        APPLY_SHOPPING_MALL_ACTION_TYPE_LIST,
        APPLY_SHOPPING_MALL_COMMIDITIES_LIST
    }
    enum NetworkToServerDataType : byte {
        INIT_CHARACTER_ID,
        SET_POSITION,
        APPLY_CAST_SKILL_BEGIN,
        APPLY_CAST_SKILL_SING_CANCEL,
        APPLY_BUILD_EQUIPMENT,
        APPLY_PICK_UP_ITEM_ON_GROUND,
        APPLY_DROP_ITEM_ONTO_GROUND,
        APPLY_USE_CONSUMABLE_ITEM,
        APPLY_USE_EQUIPMENT_ITEM,
        APPLY_SELL_ITEM_IN_BAG,
        APPLY_BUY_ITEM,
        APPLY_ACCEPT_MISSION,
        APPLY_DELIVER_MISSION,
        APPLY_CANCEL_MISSION,
        APPLY_TALK_TO_MISSION_NPC,
        APPLY_UPDATE_SKILL_LEVEL,
        APPLY_DISTRIBUTE_POINTS,
        REQUIRE_SHOPPING_MALL_ACTION_TYPE_LIST,
        REQUIRE_SHOPPING_MALL_COMMIDITIES,
        ADD_AIM_MISSION
    }
}