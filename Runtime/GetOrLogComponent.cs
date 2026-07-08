using UnityEngine;
namespace TechCosmos.ToolBox.Runtime
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

        public static T GetOrLogComponentOnChild<T>(GameObject parent, string childName) where T : Component
        {
            if (parent == null)
            {
                Debug.LogError("目标GameObject为null，无法获取子物体组件");
                return null;
            }

            if (string.IsNullOrEmpty(childName))
            {
                Debug.LogError("子物体名称为空，无法获取组件");
                return null;
            }

            Transform childTransform = parent.transform.Find(childName);
            GameObject childObject;

            if (childTransform != null)
            {
                childObject = childTransform.gameObject;
            }
            else
            {
                childObject = new GameObject(childName);
                childObject.transform.SetParent(parent.transform, worldPositionStays: false);
                Debug.Log($"{parent.name} 下未找到子物体 \"{childName}\"，已自动创建");
            }

            return GetOrLogComponent<T>(childObject);
        }
    }
}
