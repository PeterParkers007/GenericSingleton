using UnityEngine;

namespace TechCosmos.ToolBox.Runtime
{
    // �糡���־û��ķ��͵���
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
                // �糡��������
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}