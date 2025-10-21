using UnityEngine;
namespace ZJM_ArchitectureUtilities.Runtime
{
    public static class ComponentTools
    {
        public static T GetOrLogComponent<T>(GameObject targetObject) where T : Component
        {
            if (targetObject == null)
            {
                Debug.LogError("Ŀ��GameObjectΪnull���޷���ȡ���");
                return null;
            }

            T component = targetObject.GetComponent<T>();
            if (component == null)
            {
                component = targetObject.AddComponent<T>();
                Debug.Log($"{targetObject.name} ��δ�ҵ� {typeof(T).Name} ��������Զ����");
            }
            return component;
        }
    }
}
