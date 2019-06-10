using System.Collections.Generic;
using System.Numerics;
using LiteNetLib.Utils;

namespace MirRemakeBackend.Network {
    struct NO_SkillParam {
        public int m_targetNetworkId;
        public Vector2 m_direction;
        public Vector2 m_position;
        public NO_SkillParam (int targetNetId, Vector2 direction, Vector2 position) {
            m_targetNetworkId = targetNetId;
            m_direction = direction;
            m_position = position;
        }
    }
    struct NO_Status {
        public short m_id;
        public float m_value;
        public float m_time;
        public NO_Status (short id, float value, float time) {
            m_id = id;
            m_value = value;
            m_time = time;
        }
    }
    struct NO_Effect {
        public short m_animId;
        public bool m_hit;
        public bool m_isCritical;
        public int m_deltaHp;
        public int m_deltaMp;
        public NO_Effect (short animId, bool hit, bool isCritical, int dHp, int dMp) {
            m_animId = animId;
            m_hit = hit;
            m_isCritical = isCritical;
            m_deltaHp = dHp;
            m_deltaMp = dMp;
        }
    }
    struct NO_Monster {
        public int m_netId;
        public Vector2 m_position;
        public short m_monsterId;
        public NO_Monster (int netId, Vector2 pos, short monId) {
            m_netId = netId;
            m_position = pos;
            m_monsterId = monId;
        }
    }
    struct NO_Character {
        public int m_netId;
        public Vector2 m_position;
        public OccupationType m_occupation;
        public short m_level;
        public NO_Character (int netId, Vector2 pos, OccupationType ocp, short lv) {
            m_netId = netId;
            m_position = pos;
            m_occupation = ocp;
            m_level = lv;
        }
    }
    struct NO_Item {
        public long m_realId;
        public short m_itemId;
        public short m_num;
        public NO_Item (long realId, short itemId, short num) {
            m_realId = realId;
            m_itemId = itemId;
            m_num = num;
        }
    }
    struct NO_EquipmentItemInfo {
        public long m_realId;
        public byte m_strengthNum;
        public IReadOnlyList < (ActorUnitConcreteAttributeType, int) > m_enchantAttrList;
        public IReadOnlyList<short> m_inlaidGemIdList;
        public NO_EquipmentItemInfo (
            long realId,
            byte strengthNum,
            IReadOnlyList < (ActorUnitConcreteAttributeType, int) > enchantAttrList,
            IReadOnlyList<short> gemIdList
        ) {
            m_realId = realId;
            m_strengthNum = strengthNum;
            m_enchantAttrList = enchantAttrList;
            m_inlaidGemIdList = gemIdList;
        }
    }
    struct NO_Repository {
        public IReadOnlyList<NO_Item> m_itemList;
        public IReadOnlyList<NO_EquipmentItemInfo> m_equipmentInfoList;
        public NO_Repository (IReadOnlyList<NO_Item> itemList, IReadOnlyList<NO_EquipmentItemInfo> equipsList) {
            m_itemList = itemList;
            m_equipmentInfoList = equipsList;
        }
    }
    static class NetworkObjectExtensions {
        public static void Put (this NetDataWriter writer, Vector2 value) {
            writer.Put (value.X);
            writer.Put (value.Y);
        }
        public static Vector2 GetVector2 (this NetDataReader reader) {
            return new Vector2 (reader.GetFloat (), reader.GetFloat ());
        }
        public static void Put (this NetDataWriter writer, NO_SkillParam value) {
            writer.Put (value.m_targetNetworkId);
            writer.Put (value.m_direction);
            writer.Put (value.m_position);
        }
        public static NO_SkillParam GetSkillParam (this NetDataReader reader) {
            return new NO_SkillParam (reader.GetInt (), reader.GetVector2 (), reader.GetVector2 ());
        }
        public static void Put (this NetDataWriter writer, NO_Status status) {
            writer.Put (status.m_id);
            writer.Put (status.m_value);
            writer.Put (status.m_time);
        }
        public static NO_Status GetStatus (this NetDataReader reader) {
            short statusId = reader.GetShort ();
            int value = reader.GetInt ();
            float time = reader.GetFloat ();
            return new NO_Status (statusId, value, time);
        }
        public static void Put (this NetDataWriter writer, NO_Effect effect) {
            writer.Put (effect.m_animId);
            writer.Put (effect.m_hit);
            writer.Put (effect.m_isCritical);
            writer.Put (effect.m_deltaHp);
            writer.Put (effect.m_deltaMp);
        }
        public static NO_Effect GetEffect (this NetDataReader reader) {
            short animId = reader.GetShort ();
            bool isHit = reader.GetBool ();
            bool isCritical = reader.GetBool ();
            int dHp = reader.GetInt ();
            int dMp = reader.GetInt ();
            return new NO_Effect (animId, isHit, isCritical, dHp, dMp);
        }
        public static void Put (this NetDataWriter writer, NO_Monster monNo) {
            writer.Put (monNo.m_netId);
            writer.Put (monNo.m_position);
            writer.Put (monNo.m_monsterId);
        }
        public static NO_Monster GetMonster (this NetDataReader reader) {
            int netId = reader.GetInt ();
            Vector2 pos = reader.GetVector2 ();
            short monsterId = reader.GetShort ();
            return new NO_Monster (netId, pos, monsterId);
        }
        public static void Put (this NetDataWriter writer, NO_Character charNo) {
            writer.Put (charNo.m_netId);
            writer.Put (charNo.m_position);
            writer.Put ((byte) charNo.m_occupation);
            writer.Put (charNo.m_level);
        }
        public static NO_Character GetCharacter (this NetDataReader reader) {
            int netId = reader.GetInt ();
            Vector2 pos = reader.GetVector2 ();
            OccupationType ocp = (OccupationType) reader.GetByte ();
            short lv = reader.GetShort ();
            return new NO_Character (netId, pos, ocp, lv);
        }
        public static void Put (this NetDataWriter writer, NO_Item item) {
            writer.Put (item.m_realId);
            writer.Put (item.m_itemId);
            writer.Put (item.m_num);
        }
        public static NO_Item GetItem (this NetDataReader reader) {
            long realId = reader.GetLong ();
            short itemId = reader.GetShort ();
            short num = reader.GetShort ();
            return new NO_Item (realId, itemId, num);
        }
        public static void Put (this NetDataWriter writer, NO_EquipmentItemInfo equipInfo) {
            writer.Put (equipInfo.m_realId);
            writer.Put (equipInfo.m_strengthNum);
            writer.Put ((byte) equipInfo.m_enchantAttrList.Count);
            for (int i = 0; i < equipInfo.m_enchantAttrList.Count; i++) {
                writer.Put ((byte) equipInfo.m_enchantAttrList[i].Item1);
                writer.Put (equipInfo.m_enchantAttrList[i].Item2);
            }
            writer.Put ((byte) equipInfo.m_inlaidGemIdList.Count);
            for (int i = 0; i < equipInfo.m_inlaidGemIdList.Count; i++)
                writer.Put (equipInfo.m_inlaidGemIdList[i]);
        }
        public static NO_EquipmentItemInfo GetEquipmentItemInfo (this NetDataReader reader) {
            long realId = reader.GetLong ();
            byte strengthNum = reader.GetByte ();
            byte enchantAttrNum = reader.GetByte ();
            var enchantAttrList = new List < (ActorUnitConcreteAttributeType, int) > (enchantAttrNum);
            for (int i = 0; i < enchantAttrNum; i++) {
                ActorUnitConcreteAttributeType attrType = (ActorUnitConcreteAttributeType) reader.GetByte ();
                int attrValue = reader.GetInt ();
                enchantAttrList.Add ((attrType, attrValue));
            }
            byte gemNum = reader.GetByte ();
            var gemIdList = new List<short> (gemNum);
            for (int i = 0; i < gemNum; i++)
                gemIdList.Add (reader.GetShort ());
            return new NO_EquipmentItemInfo (realId, strengthNum, enchantAttrList, gemIdList);
        }
        public static void Put (this NetDataWriter writer, NO_Repository repo) {
            writer.Put ((byte) repo.m_itemList.Count);
            for (int i = 0; i < repo.m_itemList.Count; i++)
                writer.Put (repo.m_itemList[i]);
            writer.Put ((byte) repo.m_equipmentInfoList.Count);
            for (int i = 0; i < repo.m_equipmentInfoList.Count; i++)
                writer.Put (repo.m_equipmentInfoList[i]);
        }
        public static NO_Repository GetRepository (this NetDataReader reader) {
            byte itemNum = reader.GetByte ();
            var itemList = new List<NO_Item> ();
            for (int i = 0; i < itemNum; i++)
                itemList.Add (reader.GetItem ());
            byte equipNum = reader.GetByte ();
            var equipList = new List<NO_EquipmentItemInfo> ();
            for (int i = 0; i < equipNum; i++)
                equipList.Add (reader.GetEquipmentItemInfo ());
            return new NO_Repository (itemList, equipList);
        }
    }
}