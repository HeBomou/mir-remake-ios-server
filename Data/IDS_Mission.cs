using System;
using System.IO;
using LitJson;
namespace MirRemakeBackend.Data {
    interface IDS_Mission {
        DO_Mission[] GetAllMission ();
        DO_Mission[] GetAllTitleMission ();
        DO_MissionTargetKillMonsterData[] GetAllMissionTargetKillMonster ();
        DO_MissionTargetGainItemData[] GetAllMissionTargetGainItem ();
        DO_MissionTargetLevelUpSkillData[] GetAllMissionTargetLevelUpSkill ();
        DO_MissionTargetChargeAdequatelyData[] GetAllMissionTargetChargeAdequately ();

        ValueTuple<short,ValueTuple<ActorUnitConcreteAttributeType,int>[]>[] GetAllTitleIDAndAttributes();
    }
    class DS_MissionImpl : IDS_Mission {
        private JsonData m_missionDatas;
        private JsonData m_allData;
        private DO_MissionTargetKillMonsterData[] m_killMonster;
        private JsonData killMonsterData;
        private DO_MissionTargetGainItemData[] m_gainItem;
        private JsonData gainItemData;
        private DO_MissionTargetLevelUpSkillData[] m_levelUpSkill;
        private JsonData levelUpSkillData;
        private DO_MissionTargetChargeAdequatelyData[] m_chargeAdequately;
        private JsonData chargeAdequatelyData;
        private JsonData m_titleDatas;
        public DO_Mission[] GetAllMission () {
            string jsonFile = File.ReadAllText ("Data/D_Mission.json");
            m_missionDatas = JsonMapper.ToObject (jsonFile);

            DO_Mission[] missionStructs = new DO_Mission[m_missionDatas.Count];
            for (int i = 0; i < m_missionDatas.Count; i++) {
                missionStructs[i] = getMissionDatas ((short) i);
            }
            return missionStructs;
        }
        private DO_Mission getMissionDatas (short ID) {
            DO_Mission mission = new DO_Mission ();
            mission.m_id = short.Parse (m_missionDatas[ID]["MissionID"].ToString ());
            mission.m_missionOccupation = (OccupationType) Enum.Parse (typeof (OccupationType), m_missionDatas[ID]["MissionOccupation"].ToString ());
            JsonData tempData;
            mission.m_acceptingNPCID = int.Parse (m_missionDatas[ID]["AcceptingNPCID"].ToString ());
            mission.m_deliveringNPCID = int.Parse (m_missionDatas[ID]["DeliveringNPCID"].ToString ());
            tempData = m_missionDatas[ID]["ConversationsWhenAccepting"];
            /**mission.m_conversationsWhenAccepting = new String[tempData.Count];
            //for (int i = 0; i < tempData.Count; i++)
            {
                mission.m_conversationsWhenAccepting[i] = tempData[i].ToString();
            }
            tempData = m_missionDatas[ID]["ConversationsWhenDelivering"];
            mission.m_conversationWhenDelivering = new String[tempData.Count];
            for (int i = 0; i < tempData.Count; i++)
            {
                mission.m_conversationWhenDelivering[i] = tempData[i].ToString();
            }**/
            mission.m_bonusMoney = long.Parse (m_missionDatas[ID]["BonusMoney"].ToString ());
            mission.m_bonusExperience = int.Parse (m_missionDatas[ID]["BonusExperience"].ToString ());
            mission.m_levelInNeed = short.Parse (m_missionDatas[ID]["LevelInNeed"].ToString ());
            tempData = m_missionDatas[ID]["FatherMissionList"];
            mission.m_fatherMissionIdArr = new short[tempData.Count];

            for (int i = 0; i < tempData.Count; i++) {
                mission.m_fatherMissionIdArr[i] = (short.Parse (tempData[i].ToString ()));
            }
            tempData = m_missionDatas[ID]["ChildrenMissionList"];
            mission.m_childrenMissionArr = new short[tempData.Count];

            for (int i = 0; i < tempData.Count; i++) {
                mission.m_childrenMissionArr[i] = (short.Parse (tempData[i].ToString ()));
            }
            tempData = m_missionDatas[ID]["MissionTarget"];
            mission.m_missionTargetArr = new ValueTuple<MissionTargetType, short>[tempData.Count];
            for (int i = 0; i < tempData.Count; i++) {
                String tempstr1 = tempData[i][0].ToString ();
                MissionTargetType mt = (MissionTargetType) Enum.Parse (typeof (MissionTargetType), tempstr1);
                short parameter1 = short.Parse (tempData[i][1].ToString ());

                mission.m_missionTargetArr[i] = new ValueTuple<MissionTargetType, short> (mt, parameter1);

            }
            tempData = m_missionDatas[ID]["BonusItems"];
            mission.m_bonusItemIdAndNumArr = new ValueTuple<short, short>[tempData.Count];

            for (int i = 0; i < tempData.Count; i++) {
                mission.m_bonusItemIdAndNumArr[i] = (short.Parse (tempData[i].ToString ().Split (' ') [0]), short.Parse (tempData[i].ToString ().Split (' ') [1]));
            }
            return mission;
        }
        public ValueTuple<DO_MissionTargetKillMonsterData[], DO_MissionTargetGainItemData[], DO_MissionTargetLevelUpSkillData[]> GetAllMissionDatas () {
            string jsonFile = File.ReadAllText ("Data/D_MissionTarget.json");
            m_allData = JsonMapper.ToObject (jsonFile);

            killMonsterData = m_allData["KILL_MONSTER"];
            gainItemData = m_allData["GAIN_ITEM"];
            levelUpSkillData = m_allData["LEVEL_UP_SKILL"];

            m_killMonster = new DO_MissionTargetKillMonsterData[killMonsterData.Count];
            for (int i = 0; i < killMonsterData.Count; i++) {
                m_killMonster[i].m_id = short.Parse (killMonsterData[i]["ID"].ToString ());
                m_killMonster[i].m_targetMonsterId = short.Parse (killMonsterData[i]["MonsterID"].ToString ());
                m_killMonster[i].m_targetNum = short.Parse (killMonsterData[i]["Num"].ToString ());
            }

            m_gainItem = new DO_MissionTargetGainItemData[gainItemData.Count];
            for (int i = 0; i < gainItemData.Count; i++) {
                m_gainItem[i].m_id = short.Parse (gainItemData[i]["ID"].ToString ());
                m_gainItem[i].m_targetItemId = short.Parse (gainItemData[i]["ItemID"].ToString ());
                m_gainItem[i].m_targetNum = short.Parse (gainItemData[i]["Num"].ToString ());
            }

            m_levelUpSkill = new DO_MissionTargetLevelUpSkillData[levelUpSkillData.Count];
            for (int i = 0; i < levelUpSkillData.Count; i++) {
                m_levelUpSkill[i].m_id = short.Parse (levelUpSkillData[i]["ID"].ToString ());
                m_levelUpSkill[i].m_targetSkillId = short.Parse (levelUpSkillData[i]["SkillID"].ToString ());
                m_levelUpSkill[i].m_targetLevel = short.Parse (levelUpSkillData[i]["Level"].ToString ());
            }

            ValueTuple<DO_MissionTargetKillMonsterData[], DO_MissionTargetGainItemData[], DO_MissionTargetLevelUpSkillData[]> res =
                new ValueTuple<DO_MissionTargetKillMonsterData[], DO_MissionTargetGainItemData[], DO_MissionTargetLevelUpSkillData[]>
                (m_killMonster, m_gainItem, m_levelUpSkill);
            return res;
        }
        public DO_MissionTargetKillMonsterData[] GetAllMissionTargetKillMonster (){
            string jsonFile = File.ReadAllText ("Data/D_MissionTarget.json");
            m_allData = JsonMapper.ToObject (jsonFile);
            killMonsterData = m_allData["KILL_MONSTER"];
            m_killMonster = new DO_MissionTargetKillMonsterData[killMonsterData.Count];
            for (int i = 0; i < killMonsterData.Count; i++) {
                m_killMonster[i].m_id = short.Parse (killMonsterData[i]["ID"].ToString ());
                m_killMonster[i].m_targetMonsterId = short.Parse (killMonsterData[i]["MonsterID"].ToString ());
                m_killMonster[i].m_targetNum = short.Parse (killMonsterData[i]["Num"].ToString ());
            }
            return m_killMonster;
        }
        public DO_MissionTargetGainItemData[] GetAllMissionTargetGainItem (){
            string jsonFile = File.ReadAllText ("Data/D_MissionTarget.json");
            m_allData = JsonMapper.ToObject (jsonFile);
            gainItemData = m_allData["GAIN_ITEM"];
            m_gainItem = new DO_MissionTargetGainItemData[gainItemData.Count];
            for (int i = 0; i < gainItemData.Count; i++) {
                m_gainItem[i].m_id = short.Parse (gainItemData[i]["ID"].ToString ());
                m_gainItem[i].m_targetItemId = short.Parse (gainItemData[i]["ItemID"].ToString ());
                m_gainItem[i].m_targetNum = short.Parse (gainItemData[i]["Num"].ToString ());
            }
            return m_gainItem;
        }
        public DO_MissionTargetLevelUpSkillData[] GetAllMissionTargetLevelUpSkill (){
            string jsonFile = File.ReadAllText ("Data/D_MissionTarget.json");
            m_allData = JsonMapper.ToObject (jsonFile);
            levelUpSkillData = m_allData["LEVEL_UP_SKILL"];
            m_levelUpSkill = new DO_MissionTargetLevelUpSkillData[levelUpSkillData.Count];
            for (int i = 0; i < levelUpSkillData.Count; i++) {
                m_levelUpSkill[i].m_id = short.Parse (levelUpSkillData[i]["ID"].ToString ());
                m_levelUpSkill[i].m_targetSkillId = short.Parse (levelUpSkillData[i]["SkillID"].ToString ());
                m_levelUpSkill[i].m_targetLevel = short.Parse (levelUpSkillData[i]["Level"].ToString ());
            }
            return m_levelUpSkill;
        }
        public DO_MissionTargetChargeAdequatelyData[] GetAllMissionTargetChargeAdequately (){
            string jsonFile = File.ReadAllText ("Data/D_MissionTarget.json");
            m_allData = JsonMapper.ToObject (jsonFile);
            chargeAdequatelyData= m_allData["CHARGE_ADEQUATELY"];
            m_chargeAdequately = new DO_MissionTargetChargeAdequatelyData[chargeAdequatelyData.Count];
            for (int i = 0; i < chargeAdequatelyData.Count; i++) {
                m_chargeAdequately[i].m_id=short.Parse(chargeAdequatelyData[i]["ID"].ToString());
                m_chargeAdequately[i].m_amount=int.Parse(chargeAdequatelyData[i]["Amount"].ToString());
                }
            return m_chargeAdequately;
        }
        public DO_Mission[] GetAllTitleMission(){
            string jsonFile = File.ReadAllText ("Data/D_Title.json");
            m_titleDatas = JsonMapper.ToObject (jsonFile);
            int count=m_titleDatas.Count;
            DO_Mission[] res=new DO_Mission[count];
            for(int i=0;i<count;i++){
                JsonData tempData=m_titleDatas[i];
                res[i].m_id=short.Parse(tempData["TitleID"].ToString());
                res[i].m_missionOccupation = (OccupationType) Enum.Parse (typeof (OccupationType), tempData["Occupation"].ToString ());
                res[i].m_levelInNeed = short.Parse (tempData["LevelInNeed"].ToString ());
                res[i].m_missionTargetArr=new ValueTuple<MissionTargetType,short>[tempData["TitleTarget"].Count];
                res[i].m_acceptingNPCID=-1;
                res[i].m_deliveringNPCID=-1;
                res[i].m_bonusItemIdAndNumArr=new ValueTuple<short,short>[0];
                res[i].m_fatherMissionIdArr=new short[0];
                res[i].m_childrenMissionArr=new short[0];
                for(int durex=0;durex<tempData["TitleTarget"].Count;durex++){
                    res[i].m_missionTargetArr[durex].Item1=(MissionTargetType)Enum.Parse(typeof(MissionTargetType),tempData["TitleTarget"][durex][0].ToString());
                    res[i].m_missionTargetArr[durex].Item2=short.Parse(tempData["TitleTarget"][durex][1].ToString());
                }
            }
            return res;

        }
        public ValueTuple<short,ValueTuple<ActorUnitConcreteAttributeType,int>[]>[] GetAllTitleIDAndAttributes(){
            string jsonFile = File.ReadAllText ("Data/D_TitleAttribute.json");
            JsonData s_titleAttributeDatas = JsonMapper.ToObject (jsonFile);
            ValueTuple<short,ValueTuple<ActorUnitConcreteAttributeType,int>[]>[] titleAttributes=new ValueTuple<short,ValueTuple<ActorUnitConcreteAttributeType,int>[]>[s_titleAttributeDatas.Count];
            for(int i=0;i<s_titleAttributeDatas.Count;i++){
                titleAttributes[i].Item1=short.Parse(s_titleAttributeDatas[i]["TitleID"].ToString());
                titleAttributes[i].Item2=new ValueTuple<ActorUnitConcreteAttributeType,int>[s_titleAttributeDatas[i]["AttributeAttr"].Count];
                for(int j=0;j<s_titleAttributeDatas[i]["AttributeAttr"].Count;j++){
                    titleAttributes[i].Item2[j].Item1=(ActorUnitConcreteAttributeType)Enum.Parse(typeof(ActorUnitConcreteAttributeType),s_titleAttributeDatas[i]["AttributeAttr"][j].ToString().Split(' ')[0]);
                    titleAttributes[i].Item2[j].Item2=int.Parse(s_titleAttributeDatas[i]["AttributeAttr"][j].ToString().Split(' ')[1]);
                }
            }
            return titleAttributes;
        }
    }
}