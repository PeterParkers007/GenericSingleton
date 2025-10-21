using UnityEngine;
namespace ZJM_ArchitectureUtilities.Runtime
{
    public static class ComponentTools
    {
        public static T GetOrLogComponent<T>(GameObject targetObject) where T : Component
        {
            if (targetObject == null)
            {
                Debug.LogError("目标GameObject为null，无法获取组件");
                return null;
            }

            T component = targetObject.GetComponent<T>();
            if (component == null)
            {
                component = targetObject.AddComponent<T>();
                Debug.Log($"{targetObject.name} 上未找到 {typeof(T).Name} 组件，已自动添加");
            }
            return component;
        }
    }
}
