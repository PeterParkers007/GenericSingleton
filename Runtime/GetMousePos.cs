using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechCosmos.ToolBox.Runtime
{
    /// <summary>
    /// 鼠标位置获取工具类
    /// </summary>
    public static class GetMousePos
    {
        private const float DEFAULT_Z_VALUE = 10f;

        /// <summary>
        /// 获取鼠标世界坐标
        /// </summary>
        /// <param name="z">Z轴深度值</param>
        /// <returns>世界坐标系中的鼠标位置</returns>
        public static Vector3 GetWorldMousePos(float z)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            return worldPosition;
        }

        /// <summary>
        /// 获取鼠标世界坐标（使用默认Z值）
        /// </summary>
        public static Vector3 GetWorldMousePos()
        {
            return GetWorldMousePos(DEFAULT_Z_VALUE);
        }

        /// <summary>
        /// 获取鼠标世界坐标（指定相机）
        /// </summary>
        /// <param name="camera">目标相机</param>
        /// <param name="z">Z轴深度值</param>
        public static Vector3 GetWorldMousePos(Camera camera, float z = DEFAULT_Z_VALUE)
        {
            camera = camera != null ? camera : Camera.main;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = z;
            return camera.ScreenToWorldPoint(mousePosition);
        }

        /// <summary>
        /// 获取鼠标2D世界坐标
        /// </summary>
        /// <param name="z">Z轴深度值</param>
        /// <returns>2D坐标系中的鼠标位置</returns>
        public static Vector2 GetWorldMousePos2D(float z = DEFAULT_Z_VALUE)
        {
            Vector3 worldPos = GetWorldMousePos(z);
            return new Vector2(worldPos.x, worldPos.y);
        }

        /// <summary>
        /// 安全获取鼠标世界坐标（带空引用检查）
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
        /// 获取鼠标在指定平面的交点（3D场景）
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