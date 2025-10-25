using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechCosmos.ToolBox.Runtime
{
    /// <summary>
    /// ���λ�û�ȡ������
    /// </summary>
    public static class GetMousePos
    {
        private const float DEFAULT_Z_VALUE = 10f;

        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        /// <param name="z">Z�����ֵ</param>
        /// <returns>��������ϵ�е����λ��</returns>
        public static Vector3 GetWorldMousePos(float z)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            return worldPosition;
        }

        /// <summary>
        /// ��ȡ����������꣨ʹ��Ĭ��Zֵ��
        /// </summary>
        public static Vector3 GetWorldMousePos()
        {
            return GetWorldMousePos(DEFAULT_Z_VALUE);
        }

        /// <summary>
        /// ��ȡ����������ָ꣨�������
        /// </summary>
        /// <param name="camera">Ŀ�����</param>
        /// <param name="z">Z�����ֵ</param>
        public static Vector3 GetWorldMousePos(Camera camera, float z = DEFAULT_Z_VALUE)
        {
            camera = camera != null ? camera : Camera.main;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = z;
            return camera.ScreenToWorldPoint(mousePosition);
        }

        /// <summary>
        /// ��ȡ���2D��������
        /// </summary>
        /// <param name="z">Z�����ֵ</param>
        /// <returns>2D����ϵ�е����λ��</returns>
        public static Vector2 GetWorldMousePos2D(float z = DEFAULT_Z_VALUE)
        {
            Vector3 worldPos = GetWorldMousePos(z);
            return new Vector2(worldPos.x, worldPos.y);
        }

        /// <summary>
        /// ��ȫ��ȡ����������꣨�������ü�飩
        /// </summary>
        public static Vector3 GetWorldMousePosSafe(float z = DEFAULT_Z_VALUE)
        {
            if (Camera.main == null)
            {
                Debug.LogError("[GetMousePos] Main Camera not found!");
                return Vector3.zero;
            }
            return GetWorldMousePos(z);
        }

        /// <summary>
        /// ��ȡ�����ָ��ƽ��Ľ��㣨3D������
        /// </summary>
        public static Vector3 GetMousePosOnPlane(Plane plane)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
            return Vector3.zero;
        }
    }
}