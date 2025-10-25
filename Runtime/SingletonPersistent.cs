using UnityEngine;

namespace TechCosmos.ToolBox.Runtime
{
    // 跨场景持久化的泛型单例
    public class SingletonPersistent<T> : MonoBehaviour where T : SingletonPersistent<T>
    {
        private static T instance;
        public static T Instance
        {
            get { return instance; }
        }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = (T)this;
                // 跨场景不销毁
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}