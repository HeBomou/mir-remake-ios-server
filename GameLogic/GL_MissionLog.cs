using MirRemakeBackend.Entity;
using MirRemakeBackend.Network;

namespace MirRemakeBackend.GameLogic {
    /// <summary>
    /// 管理消息
    /// </summary>
    class GL_MissionLog : GameLogicBase {
        public static GL_MissionLog s_instance;
        public GL_MissionLog (INetworkService netService) : base (netService) { }
        public override void Tick (float dT) {
            EM_Log.s_instance.NextTick ();
        }
        public override void NetworkTick () { }
        public void NotifyLog (GameLogType type, int netId, int parm1 = 0, int parm2 = 0, int parm3 = 0) {
            var logs = EM_Log.s_instance.GetRawLogsCurTick ();
            logs.Add (EM_Log.s_instance.CreateLog (type, netId, parm1, parm2, parm3));
        }
    }
}