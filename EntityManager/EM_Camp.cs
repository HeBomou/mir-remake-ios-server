using MirRemakeBackend.Entity;

namespace MirRemakeBackend.EntityManager {
    /// <summary>
    /// 处理阵营信息, 组队
    /// </summary>
    class EM_Camp : EntityManagerBase {
        public static EM_Camp s_instance;
        public CampType GetCampType (E_ActorUnit self, E_ActorUnit target) {
            if (self == target)
                return CampType.SELF;
            return CampType.ENEMY;
        }
    }
}