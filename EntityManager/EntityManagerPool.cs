using System.Collections.Generic;

namespace MirRemakeBackend {
    /// <summary>
    /// 内存池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class EntityManagerPool<T> where T : class, new () {
        private Stack<T> m_pool = new Stack<T> ();
        private int m_Size;
        public EntityManagerPool (int Size) {
            for (int i = 0; i < Size; i++)
                m_pool.Push (new T ());
            m_Size = Size;
        }
        public T GetInstance () {
            T res = null;
            if (m_pool.TryPop (out res))
                return res;
            return new T ();
        }
        public void RecycleInstance (T obj) {
            if (m_pool.Count < m_Size)
                m_pool.Push (obj);
        }
    }
    static class EntityManagerPoolInstance {
        private const int c_characterPoolSize = 400;
        private const int c_skillPoolSize = 5000;
        public static EntityManagerPool<E_Character> s_characterPool = new EntityManagerPool<E_Character> (c_characterPoolSize);
        public static EntityManagerPool<E_Skill> s_skillPool = new EntityManagerPool<E_Skill> (c_skillPoolSize);
    }
}