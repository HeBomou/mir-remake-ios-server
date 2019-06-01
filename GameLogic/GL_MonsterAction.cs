using System;
using System.Collections.Generic;
using System.Numerics;
using MirRemakeBackend.DataEntity;
using MirRemakeBackend.Entity;
using MirRemakeBackend.EntityManager;
using MirRemakeBackend.Network;

namespace MirRemakeBackend.GameLogic {
    /// <summary>
    /// 控制Monster的AI
    /// </summary>
    partial class GL_MonsterAction : GameLogicBase {
        public static GL_MonsterAction s_instance;
        private Dictionary<int, MFSM> m_mfsmDict = new Dictionary<int, MFSM> ();
        public GL_MonsterAction (INetworkService netService) : base (netService) {
            var monEn = EM_ActorUnit.s_instance.GetMonsterEn ();
            while (monEn.MoveNext ()) {
                var mfsm = new MFSM (monEn.Current.Value);
                m_mfsmDict.Add (monEn.Current.Key, mfsm);
            }
        }
        public override void Tick (float dT) {
            var mfsmEn = m_mfsmDict.GetEnumerator ();
            while (mfsmEn.MoveNext ())
                mfsmEn.Current.Value.Tick (dT);
        }
        public override void NetworkTick () { }
        public void MFSMCastSkillBegin (int netId, E_MonsterSkill skill) {
            // TODO: 通知Client
        }
        public void MFSMSkillSettle (E_Monster monster, E_MonsterSkill skill, SkillParam sp) {
            GL_BattleSettle.s_instance.NotifySkillSettle (monster, skill, sp);
        }
        public void MFSMDead (E_Monster monster) { }
        public void MFSMRespawn (E_Monster monster) { }
    }
}