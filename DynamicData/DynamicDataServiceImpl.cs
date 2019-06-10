﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using LitJson;
namespace MirRemakeBackend.DynamicData {
    class DynamicDataServiceImpl : IDDS_Item, IDDS_Skill, IDDS_Mission, IDDS_Character {
        private SqlConfig sqlConfig;
        private SQLPool pool;
        public DynamicDataServiceImpl () {
            sqlConfig = new SqlConfig ();
            sqlConfig.username = "root";
            sqlConfig.pwd = "root";
            sqlConfig.database = "legend";
            sqlConfig.server = "localhost";
            pool = new SQLPool (sqlConfig);
        }
        public List<DDO_Item> GetBagByCharacterId (int charId) {
            string cmd;
            DataSet ds = new DataSet ();
            cmd = "select * from `item` where charid=" + charId + " and place=\"BAG\";";
            string database = "legend";
            pool.ExecuteSql (database, cmd, ds);
            DataTable dt = ds.Tables[0];
            List<DDO_Item> res = new List<DDO_Item> ();
            for (int i = 0; i < dt.Rows.Count; i++) {
                short realid = short.Parse (dt.Rows[i]["realid"].ToString ());
                short itemid = short.Parse (dt.Rows[i]["itemid"].ToString ());
                short num = short.Parse (dt.Rows[i]["num"].ToString ());
                ItemPlace place = ItemPlace.BAG;
                short pos = short.Parse (dt.Rows[i]["position"].ToString ());
                res.Add (new DDO_Item (realid, itemid, charId, num, place, pos));
            }
            return res;
        }
        public List<DDO_Item> GetStoreHouseByCharacterId (int charId) {
            string cmd;
            DataSet ds = new DataSet ();
            cmd = "select * from `item` where charid=" + charId + " and place=\"STORE_HOUSE\";";
            string database = "legend";
            pool.ExecuteSql (database, cmd, ds);
            DataTable dt = ds.Tables[0];
            List<DDO_Item> res = new List<DDO_Item> ();
            for (int i = 0; i < dt.Rows.Count; i++) {
                short realid = short.Parse (dt.Rows[i]["realid"].ToString ());
                short itemid = short.Parse (dt.Rows[i]["itemid"].ToString ());
                short num = short.Parse (dt.Rows[i]["num"].ToString ());
                ItemPlace place = ItemPlace.STORE_HOUSE;
                short pos = short.Parse (dt.Rows[i]["position"].ToString ());
                res.Add (new DDO_Item (realid, itemid, charId, num, place, pos));
            }
            return res;
        }
        public List<DDO_Item> GetEquipmentRegionByCharacterId (int charId) {
            string cmd;
            DataSet ds = new DataSet ();
            cmd = "select * from `item` where charid=" + charId + " and `position`=\"BAG\";";
            string database = "legend";
            pool.ExecuteSql (database, cmd, ds);
            DataTable dt = ds.Tables[0];
            List<DDO_Item> res = new List<DDO_Item> ();
            for (int i = 0; i < dt.Rows.Count; i++) {
                short realid = short.Parse (dt.Rows[i]["realid"].ToString ());
                short itemid = short.Parse (dt.Rows[i]["itemid"].ToString ());
                short num = short.Parse (dt.Rows[i]["num"].ToString ());
                ItemPlace place = ItemPlace.STORE_HOUSE;
                short pos = short.Parse (dt.Rows[i]["position"].ToString ());
                res.Add (new DDO_Item (realid, itemid, charId, num, place, pos));
            }
            return res;
        }
        public List<DDO_EquipmentInfo> GetAllEquipmentByCharacterId (int charId) {
            string cmd;
            DataSet ds = new DataSet ();
            cmd = "select * from `equipment` where charid=" + charId + ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd, ds);
            DataTable dt = ds.Tables[0];
            List<DDO_EquipmentInfo> res = new List<DDO_EquipmentInfo> ();
            for (int i = 0; i < dt.Rows.Count; i++) {
                DDO_EquipmentInfo equipment = new DDO_EquipmentInfo ();
                equipment.m_strengthNum = byte.Parse (dt.Rows[i]["strength_num"].ToString ());
                equipment.m_holeNum = short.Parse (dt.Rows[i]["hole_num"].ToString ());
                equipment.m_realId = short.Parse (dt.Rows[i]["realid"].ToString ());
                string gems = dt.Rows[i]["gem_list"].ToString ();
                equipment.m_inlaidGemIdList = new List<short> ();
                if (gems.Length != 0) {
                    for (int j = 0; j < gems.Split (' ').Length; j++) {
                        equipment.m_inlaidGemIdList.Add (short.Parse (gems.Split (' ') [j]));
                    }
                }
                JsonData attr = JsonMapper.ToObject (dt.Rows[i]["enchant_attr"].ToString ());
                equipment.m_enchantAttr = GetAttr (attr);
                res.Add (equipment);
            }
            return res;
        }
        public void UpdateItem (DDO_Item item) {
            string cmd;
            cmd = "update `item` set num=" + item.m_num + ",place=" + item.m_place.ToString () + ",`position`=" + item.m_position + " where itemid=" + item.m_itemId + " and realid=" + item.m_realId + ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd);
        }
        public void DeleteItemByRealId (long realId) {
            string cmd;
            cmd = "delete from `item` where realid=" + realId + ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd);
        }
        public long InsertItem (DDO_Item item) {
            string cmd;
            DataSet ds = new DataSet ();
            cmd = "insert into `item` values(null," + item.m_itemId + "," + item.m_characterId + "," + item.m_num + ",\"" + item.m_place.ToString () + "\"," + item.m_position + ");select last_insert_id();";
            string database = "legend";
            pool.ExecuteSql (database, cmd, ds);
            return int.Parse (ds.Tables[0].Rows[0]["last_insert_id()"].ToString ());
        }
        public void UpdateEquipmentInfo (DDO_EquipmentInfo eq) {
            string cmd;
            string gems;
            if (eq.m_inlaidGemIdList.Count != 0) {
                gems = eq.m_inlaidGemIdList[0].ToString ();
                for (int i = 0; i < eq.m_inlaidGemIdList.Count; i++) {
                    gems = gems + " " + eq.m_inlaidGemIdList[i].ToString ();
                }
            } else {
                gems = "";
            }
            string enchantAttr = GetString (eq.m_enchantAttr);
            cmd = "update `equipment` set `charid`=" + eq.m_characterId + ", strength_num=" + eq.m_strengthNum + ", gem_list=\"" + gems + "\",enchant_attr=\"" + enchantAttr + "\" where realid="+eq.m_realId+ ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd); 
        }
        public void DeleteEquipmentInfoByRealId (long realId) {
            string cmd="delete from `equipment` where `realid`="+realId+";";
            string database="legend";
            pool.ExecuteSql(database,cmd); 
        }
        public void InsertEquipmentInfo (DDO_EquipmentInfo eq) {
            string cmd;
            string gems;
            if (eq.m_inlaidGemIdList.Count != 0) {
                gems = eq.m_inlaidGemIdList[0].ToString ();
                for (int i = 0; i < eq.m_inlaidGemIdList.Count; i++) {
                    gems = gems + " " + eq.m_inlaidGemIdList[i].ToString ();
                }
            } else {
                gems = "";
            }
            string enchantAttr = GetString (eq.m_enchantAttr);
            cmd = "insert into `equipment` values(null," + eq.m_characterId + "," + eq.m_strengthNum + "," + gems + ",\"" + enchantAttr + "\"," + eq.m_holeNum + ");";
            string database = "legend";
            pool.ExecuteSql (database, cmd);
        }

        public List<DDO_Skill> GetSkillListByCharacterId (int charId) {
            string cmd;
            DataSet ds = new DataSet ();
            cmd = "select * from skill where charid=" + charId + ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd, ds);
            DataTable dt = ds.Tables[0];
            List<DDO_Skill> res = new List<DDO_Skill> ();
            for (int i = 0; i < dt.Rows.Count; i++) {
                DDO_Skill skill = new DDO_Skill ();
                skill.m_skillId = short.Parse (dt.Rows[i]["skillid"].ToString ());
                skill.m_masterly = int.Parse (dt.Rows[i]["masterly"].ToString ());
                skill.m_skillLevel = short.Parse (dt.Rows[i]["level"].ToString ());
                res.Add (skill);
            }
            return res;
        }
        public void UpdateSkill (DDO_Skill ddo) {
            int charId = ddo.m_characterId;
            string cmd;
            DataSet ds = new DataSet ();
            cmd = "select * from skill where charid=" + charId + " and skillid=" + ddo.m_skillId + ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd, ds);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count != 0) {
                cmd = "update skill set masterly=" + ddo.m_masterly + ", level=" + ddo.m_skillLevel + " where charid=" + charId + " and skillid=" + ddo.m_skillId + ";";
            } else {
                cmd = "insert into skill values(null," + ddo.m_skillId + "," + charId + "," + ddo.m_masterly + "," + ddo.m_skillLevel + ")";
            }
            pool.ExecuteSql (database, cmd);
        }

        public int CreateCharacter (OccupationType occupation) {
            string cmd;
            DataSet ds = new DataSet ();
            DataTable dt = new DataTable ();
            cmd = "insert into `character` values (null,\"" + occupation.ToString () + "\",1,0,\"0 0\",\"0 0 0 0\");select last_insert_id();";
            string database = "legend";
            pool.ExecuteSql (database, cmd, ds);
            dt = ds.Tables[0];
            return int.Parse (dt.Rows[0]["last_insert_id()"].ToString ());
        }
        public DDO_Character GetCharacterById (int characterId) {
            DDO_Character character = new DDO_Character ();
            string cmd;
            DataSet ds = new DataSet ();
            cmd = "select * from `character` where characterid=" + characterId + ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd, ds);
            DataTable dt = ds.Tables[0];
            character.m_currencyArr = new ValueTuple<CurrencyType, long>[2];
            character.m_currencyArr[0] = new ValueTuple<CurrencyType, long> (CurrencyType.CHARGE, long.Parse (dt.Rows[0]["currency"].ToString ().Split (' ') [0]));
            character.m_currencyArr[1] = new ValueTuple<CurrencyType, long> (CurrencyType.VIRTUAL, long.Parse (dt.Rows[0]["currency"].ToString ().Split (' ') [1]));
            character.m_distributedMainAttrPointArr = new ValueTuple<ActorUnitMainAttributeType, short>[4];
            character.m_distributedMainAttrPointArr[0] = new ValueTuple<ActorUnitMainAttributeType, short> (ActorUnitMainAttributeType.STRENGTH, short.Parse (dt.Rows[0]["giftpoints"].ToString ().Split (' ') [0]));
            character.m_distributedMainAttrPointArr[1] = new ValueTuple<ActorUnitMainAttributeType, short> (ActorUnitMainAttributeType.AGILITY, short.Parse (dt.Rows[0]["giftpoints"].ToString ().Split (' ') [1]));
            character.m_distributedMainAttrPointArr[2] = new ValueTuple<ActorUnitMainAttributeType, short> (ActorUnitMainAttributeType.INTELLIGENCE, short.Parse (dt.Rows[0]["giftpoints"].ToString ().Split (' ') [2]));
            character.m_distributedMainAttrPointArr[3] = new ValueTuple<ActorUnitMainAttributeType, short> (ActorUnitMainAttributeType.SPIRIT, short.Parse (dt.Rows[0]["giftpoints"].ToString ().Split (' ') [3]));
            character.m_level = short.Parse (dt.Rows[0]["level"].ToString ());
            character.m_occupation = (OccupationType) Enum.Parse (typeof (OccupationType), dt.Rows[0]["occupation"].ToString ());
            character.m_experience = int.Parse (dt.Rows[0]["experience"].ToString ());
            character.m_characterId = int.Parse (dt.Rows[0]["characterid"].ToString ());
            return character;
        }
        public void UpdateCharacter (DDO_Character charObj) {
            string cmd;
            string currencyArr = "\"" + charObj.m_currencyArr[0].Item2.ToString () + "," + charObj.m_currencyArr[1].Item2.ToString () + "\"";
            string giftPoints = "\"" + charObj.m_distributedMainAttrPointArr[0].Item2.ToString () + "," +
                charObj.m_distributedMainAttrPointArr[1].Item2.ToString () + "," +
                charObj.m_distributedMainAttrPointArr[2].Item2.ToString () + "," +
                charObj.m_distributedMainAttrPointArr[3].Item2.ToString () + "\"";
            cmd = "update `character` set characterid=" + charObj.m_characterId + ",occupation=\"" + charObj.m_occupation.ToString () + "\",level=" + charObj.m_level + ",expericence=" +
                charObj.m_experience + ",currency=" + currencyArr + ",giftpoints=" + giftPoints + " where characterid=" + charObj.m_characterId + ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd);
        }

        public List<DDO_Mission> GetMissionListByCharacterId (int charId) {
            string cmd;
            DataSet ds = new DataSet ();
            cmd = "select * from mission where charid=" + charId + ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd, ds);
            DataTable dt = ds.Tables[0];
            List<DDO_Mission> missions = new List<DDO_Mission> ();
            for (int i = 0; i < dt.Rows.Count; i++) {
                DDO_Mission mission = new DDO_Mission ();
                mission.m_missionId = short.Parse (dt.Rows[i]["missionid"].ToString ());
                mission.m_characterId=short.Parse(dt.Rows[i]["charid"].ToString());                mission.m_missionTargetProgressList = new List<int> ();
                string[] targets = dt.Rows[i]["targets"].ToString ().Split (' ');
                for (int j = 0; j < targets.Length; j++) {
                    mission.m_missionTargetProgressList.Add (int.Parse (targets[j]));
                }
            }
            return missions;
        }
        public void InsertMission (DDO_Mission ddo) {
            string cmd;
            string target = ddo.m_missionTargetProgressList[0].ToString ();
            for (int i = 1; i < ddo.m_missionTargetProgressList.Count; i++) {
                target = target + " " + ddo.m_missionTargetProgressList[i].ToString ();
            }
            cmd = "insert into mission values(null," + ddo.m_missionId +","+ddo.m_characterId +",\"" + target + "\""  + ");";
            string database = "legend";
            pool.ExecuteSql (database, cmd);
        }
        public void UpdateMission (DDO_Mission ddo) {
            string cmd;
            string target = ddo.m_missionTargetProgressList[0].ToString ();
            for (int i = 1; i < ddo.m_missionTargetProgressList.Count; i++) {
                target = target + " " + ddo.m_missionTargetProgressList[i].ToString ();
            }
            cmd = "update mission set targets=\"" + target + "\" where charid=" + ddo.m_characterId + " and missionid=" + ddo.m_missionId + ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd);
        }
        public void DeleteMission (short missionId, int charId) {
            string cmd;
            cmd = "delete from `item` where charid=" + charId + " and missionid=" + missionId + ";";
            string database = "legend";
            pool.ExecuteSql (database, cmd);

        }

        private ValueTuple<ActorUnitConcreteAttributeType, int>[] GetAttr (JsonData attr) {
            ValueTuple<ActorUnitConcreteAttributeType, int>[] res = new ValueTuple<ActorUnitConcreteAttributeType, int>[attr.Count];
            for (int j = 0; j < attr.Count; j++) {
                res[j] = new ValueTuple<ActorUnitConcreteAttributeType, int>
                    ((ActorUnitConcreteAttributeType) Enum.Parse (typeof (ActorUnitConcreteAttributeType), attr[j].ToString ().Split (' ') [0]),
                        int.Parse (attr[j].ToString ().Split (' ') [1]));
            }
            return res;
        }
        private string GetString (ValueTuple<ActorUnitConcreteAttributeType, int>[] ps) {
            if (ps.Length == 0) {
                return new String ("");
            }
            String res = new String ("");
            res = "[\"" + ps[0].Item1.ToString () + "," + ps[0].Item2.ToString () + "\"";
            for (int i = 1; i < ps.Length; i++) {
                res = res + ",\"" + ps[0].Item1.ToString () + "," + ps[0].Item2.ToString () + "\"";
            }

            res = res + "]";
            return res;
        }
    }
}