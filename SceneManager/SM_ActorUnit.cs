using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirRemake {
    class SM_ActorUnit {
        public static SM_ActorUnit s_instance = new SM_ActorUnit ();
        private INetworkService m_networkService;
        private HashSet<int> m_playerNetIdSet = new HashSet<int> ();
        private Stack<E_ActorUnit> m_networkIdBodyToDisappearStack = new Stack<E_ActorUnit> ();
        private Dictionary<int, E_ActorUnit> m_networkIdAndActorUnitDict = new Dictionary<int, E_ActorUnit> ();
        private const float c_monsterRefreshTime = 15f;
        private Dictionary<int, Vector2> m_networkIdAndMonsterPosDict = new Dictionary<int, Vector2> ();
        private Dictionary<int, short> m_networkIdAndMonsterIdDict = new Dictionary<int, short> ();
        private Dictionary<int, MyTimer.Time> m_networkIdAndMonsterRefreshTimeDict = new Dictionary<int, MyTimer.Time> ();
        public SM_ActorUnit () {
            // TODO: 用于测试
            int monsterNetId;
            monsterNetId = NetworkIdManager.GetNewActorUnitNetworkId ();
            m_networkIdAndMonsterIdDict[monsterNetId] = 0;
            m_networkIdAndMonsterPosDict[monsterNetId] = new Vector2 (-1, 0);
            m_networkIdAndMonsterRefreshTimeDict[monsterNetId] = new MyTimer.Time (0, 1f);
            monsterNetId = NetworkIdManager.GetNewActorUnitNetworkId ();
            m_networkIdAndMonsterIdDict[monsterNetId] = 1;
            m_networkIdAndMonsterPosDict[monsterNetId] = new Vector2 (-3, 1);
            m_networkIdAndMonsterRefreshTimeDict[monsterNetId] = new MyTimer.Time (0, 10f);
        }
        public void Init (INetworkService netService) {
            m_networkService = netService;
        }
        public E_ActorUnit GetActorUnitByNetworkId (int networkId) {
            E_ActorUnit res = null;
            m_networkIdAndActorUnitDict.TryGetValue (networkId, out res);
            return res;
        }
        private List<E_ActorUnit> GetActorUnitArrByNetworkIdArr (int[] networkIdArr) {
            List<E_ActorUnit> res = new List<E_ActorUnit> (networkIdArr.Length);
            foreach (var netId in networkIdArr) {
                E_ActorUnit unit = null;
                m_networkIdAndActorUnitDict.TryGetValue (netId, out unit);
                if (unit != null)
                    res.Add (unit);
            }
            return res;
        }
        public List<E_ActorUnit> GetActorUnitsInSectorRange (E_ActorUnit self, Vector2 center, Vector2 dir, float range, float radian, CampType targetCamp, byte num) {
            // TODO: 解决非圆扇形的作用目标判定
            List<E_ActorUnit> res = new List<E_ActorUnit> ();
            var unitEn = m_networkIdAndActorUnitDict.Values.GetEnumerator ();
            while (unitEn.MoveNext ()) {
                if (CheckCampMatch (self, unitEn.Current, targetCamp) && (center - unitEn.Current.m_Position).magnitude < range + unitEn.Current.m_CoverRadius)
                    res.Add (unitEn.Current);
            }
            return GetNearestUnits (center, res, num);
        }
        public List<E_ActorUnit> GetActorUnitsInCircleRange (E_ActorUnit self, Vector2 center, float range, CampType targetCamp, byte num) {
            List<E_ActorUnit> res = new List<E_ActorUnit> ();
            var unitEn = m_networkIdAndActorUnitDict.Values.GetEnumerator ();
            while (unitEn.MoveNext ()) {
                if (CheckCampMatch (self, unitEn.Current, targetCamp) && (center - unitEn.Current.m_Position).magnitude < range + unitEn.Current.m_CoverRadius)
                    res.Add (unitEn.Current);
            }
            return GetNearestUnits (center, res, num);
        }
        public List<E_ActorUnit> GetActorUnitsInLineRange (E_ActorUnit self, Vector2 center, Vector2 dir, float distance, float width, CampType targetCamp, byte num) {
            List<E_ActorUnit> res = new List<E_ActorUnit> ();
            var unitEn = m_networkIdAndActorUnitDict.Values.GetEnumerator ();
            while (unitEn.MoveNext ()) {
                if (CheckCampMatch (self, unitEn.Current, targetCamp) && false) // TODO: 解决直线的作用目标判定
                    res.Add (unitEn.Current);
            }
            return GetNearestUnits (center, res, num);
        }
        private List<E_ActorUnit> GetNearestUnits (Vector2 center, List<E_ActorUnit> units, byte num) {
            if (units.Count <= num) return units;
            // TODO: 对units进行排序并剔除多余的unit
            // units.Sort();
            return units;
        }
        private List<int> GetPlayerInSightIdList (E_ActorUnit self, bool includeSelf) {
            // TODO: 处理视野问题
            List<int> res = new List<int> ();
            foreach (var netId in m_playerNetIdSet) {
                if (includeSelf || netId != self.m_networkId)
                    res.Add (netId);
            }
            return res;
        }
        public bool CheckCampMatch (E_ActorUnit self, E_ActorUnit target, CampType camp) {
            // TODO: 解决组队问题
            switch (camp) {
                case CampType.SELF:
                    return self == target;
                case CampType.FRIEND:
                    return false;
                case CampType.ENEMY:
                    return self != target;
            }
            return false;
        }
        public void NotifyUnitDead (int killerNetId, E_ActorUnit deadUnit) {
            m_networkService.SendServerCommand (new SC_ApplyAllDead (GetPlayerInSightIdList (deadUnit, true), killerNetId, deadUnit.m_networkId));
        }
        public void NotifyUnitBodyDisappear (E_ActorUnit deadUnit) {
            // 死亡单位移除准备
            m_networkIdBodyToDisappearStack.Push (deadUnit);
        }
        public void Tick (float dT) {
            // 每个单位的Tick
            var unitEn = m_networkIdAndActorUnitDict.GetEnumerator ();
            while (unitEn.MoveNext ())
                unitEn.Current.Value.Tick (dT);

            // 移除消失的尸体
            E_ActorUnit bodyToDisappear;
            while (m_networkIdBodyToDisappearStack.TryPop(out bodyToDisappear)) {
                m_networkIdAndActorUnitDict.Remove (bodyToDisappear.m_networkId);
                if (bodyToDisappear.m_ActorUnitType == ActorUnitType.Monster) {
                    MyTimer.Time refreshTime = MyTimer.s_CurTime;
                    refreshTime.Tick (c_monsterRefreshTime);
                    m_networkIdAndMonsterRefreshTimeDict.Add (bodyToDisappear.m_networkId, refreshTime);
                }
            }

            // 处理怪物刷新
            List<int> monsterIdToRefreshList = new List<int> ();
            var monsterDeathTimeEn = m_networkIdAndMonsterRefreshTimeDict.GetEnumerator ();
            while (monsterDeathTimeEn.MoveNext ())
                if (MyTimer.CheckTimeUp (monsterDeathTimeEn.Current.Value))
                    monsterIdToRefreshList.Add (monsterDeathTimeEn.Current.Key);
            for (int i = 0; i < monsterIdToRefreshList.Count; i++) {
                int monsterIdToRefresh = monsterIdToRefreshList[i];
                m_networkIdAndMonsterRefreshTimeDict.Remove (monsterIdToRefresh);
                m_networkIdAndActorUnitDict.Add (monsterIdToRefresh, new E_Monster (monsterIdToRefresh, m_networkIdAndMonsterIdDict[monsterIdToRefresh], m_networkIdAndMonsterPosDict[monsterIdToRefresh]));
            }
        }
        public void NetworkTick () {
            var selfKeyEn = m_playerNetIdSet.GetEnumerator ();

            while (selfKeyEn.MoveNext ()) {
                var selfNetId = selfKeyEn.Current;
                var self = (E_Character) m_networkIdAndActorUnitDict[selfNetId];

                // 发送其他unit视野信息
                List<int> playerNetIdList = new List<int> ();
                List<int> monsterNetIdList = new List<int> ();
                var otherUnitEn = m_networkIdAndActorUnitDict.GetEnumerator ();
                while (otherUnitEn.MoveNext ()) {
                    if (otherUnitEn.Current.Key == selfNetId) continue;
                    switch (otherUnitEn.Current.Value.m_ActorUnitType) {
                        case ActorUnitType.Monster:
                            monsterNetIdList.Add (otherUnitEn.Current.Key);
                            break;
                        case ActorUnitType.Player:
                            playerNetIdList.Add (otherUnitEn.Current.Key);
                            break;
                    }
                }
                m_networkService.SendServerCommand (new SC_ApplyOtherActorUnitInSight (new List<int> { selfNetId }, ActorUnitType.Player, playerNetIdList));
                m_networkService.SendServerCommand (new SC_ApplyOtherActorUnitInSight (new List<int> { selfNetId }, ActorUnitType.Monster, monsterNetIdList));

                // 发送视野内所有单位的位置信息
                List<int> unitNetIdList = new List<int> ();
                List<Vector2> posList = new List<Vector2> ();
                var allUnitEn = m_networkIdAndActorUnitDict.GetEnumerator ();
                while (allUnitEn.MoveNext ())
                    if (allUnitEn.Current.Key != selfNetId) {
                        unitNetIdList.Add (allUnitEn.Current.Key);
                        posList.Add (allUnitEn.Current.Value.m_Position);
                    }
                m_networkService.SendServerCommand (new SC_SetOtherPosition (new List<int> { selfNetId }, unitNetIdList, posList));

                // 发送视野内所有单位的HP与MP
                unitNetIdList.Clear ();
                List<Dictionary<ActorUnitConcreteAttributeType, int>> HPMPList = new List<Dictionary<ActorUnitConcreteAttributeType, int>> ();
                allUnitEn = m_networkIdAndActorUnitDict.GetEnumerator ();
                while (allUnitEn.MoveNext ()) {
                    var allUnit = allUnitEn.Current.Value;
                    unitNetIdList.Add (allUnitEn.Current.Key);
                    HPMPList.Add (allUnit.m_concreteAttributeDict);
                }
                m_networkService.SendServerCommand (new SC_SetAllHPAndMP (new List<int> { selfNetId }, unitNetIdList, HPMPList));
            }
        }
        /// <summary>
        /// 在新的玩家连接到服务器后调用
        /// 为它分配并返回一个NetworkId
        /// </summary>
        /// <returns></returns>
        public int CommandAssignNetworkId () {
            return NetworkIdManager.GetNewActorUnitNetworkId ();
        }
        public void CommandRemoveCharacter (int netId) {
            NetworkIdManager.RemoveActorUnitNetworkId (netId);
            m_networkIdAndActorUnitDict.Remove (netId);
            m_playerNetIdSet.Remove (netId);
        }
        public void CommandInitCharacterPlayerId (int netId, int playerId) {
            E_Character newChar = new E_Character (netId, playerId);
            m_networkIdAndActorUnitDict[netId] = newChar;
            m_playerNetIdSet.Add (netId);
            short[] skillIdArr;
            short[] skillLvArr;
            int[] skillMasterlyArr;
            newChar.GetAllLearnedSkill (out skillIdArr, out skillLvArr, out skillMasterlyArr);

            m_networkService.SendServerCommand (new SC_InitSelfInfo (new List<int> { netId }, newChar.m_Level, newChar.m_Experience, skillIdArr, skillLvArr, skillMasterlyArr));
        }
        public void CommandSetPosition (int netId, Vector2 pos) {
            m_networkIdAndActorUnitDict[netId].m_Position = pos;
        }
        public void CommandApplyCastSkillBegin (int netId, short skillId, Vector2 tarPos, SkillParam parm) { }
        public void CommandApplyCastSkillSingCancel (int netId) {

        }
        public void CommandApplyCastSkillSettle (int netId, short skillId, int[] tarIdArr) {
            E_Skill skill = new E_Skill (skillId);
            KeyValuePair<int, E_Status[]>[] statusPairArr;
            E_ActorUnit unit = GetActorUnitByNetworkId (netId);
            if (unit == null) return;
            unit.ApplyCastSkill (skill, GetActorUnitArrByNetworkIdArr (tarIdArr), out statusPairArr);
            m_networkService.SendServerCommand (new SC_ApplyAllEffect (GetPlayerInSightIdList (m_networkIdAndActorUnitDict[netId], true), skill.m_skillEffect.m_animId, (byte) skill.m_skillEffect.m_StatusAttachNum, statusPairArr));
        }
    }
}