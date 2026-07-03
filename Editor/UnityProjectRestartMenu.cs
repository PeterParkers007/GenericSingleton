using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TechCosmos.ToolBox.Editor
{
    /// <summary>
    /// 编辑器菜单：一键重启当前 Unity 项目。
    /// </summary>
    public static class UnityProjectRestartMenu
    {
        const string MenuPath = "Tech-Cosmos/ToolBox/重启 Unity 项目";

        /// <summary>等待当前 Unity 退出后再启动（秒）。</summary>
        const int RestartDelaySeconds = 2;

        [MenuItem(MenuPath, false, 1000)]
        public static void RestartUnityProject()
        {
            if (!EditorUtility.DisplayDialog(
                    "重启 Unity 项目",
                    "将尝试保存已修改的场景与资源，然后重新启动当前项目。\n\n是否继续？",
                    "重启",
                    "取消"))
            {
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            AssetDatabase.SaveAssets();

            var projectPath = Path.GetDirectoryName(Application.dataPath);
            if (string.IsNullOrEmpty(projectPath))
            {
                Debug.LogError("[ToolBox] 无法解析项目路径，重启已取消。");
                return;
            }

            var unityEditorPath = EditorApplication.applicationPath;
            if (string.IsNullOrEmpty(unityEditorPath) || !File.Exists(unityEditorPath))
            {
                Debug.LogWarning("[ToolBox] 未找到 Unity 编辑器路径，改用 OpenProject 重新打开。");
                EditorApplication.OpenProject(projectPath);
                return;
            }

            if (!TryLaunchDetachedRestartHelper(unityEditorPath, projectPath, out var error))
            {
                Debug.LogError($"[ToolBox] 启动重启助手失败：{error}，改用 OpenProject。");
                EditorApplication.OpenProject(projectPath);
                return;
            }

            Debug.Log($"[ToolBox] 正在重启项目：{projectPath}");
            EditorApplication.Exit(0);
        }

        [MenuItem(MenuPath, true)]
        public static bool RestartUnityProjectValidate()
        {
            return !EditorApplication.isCompiling && !EditorApplication.isUpdating;
        }

        /// <summary>
        /// 通过独立 cmd 进程延迟启动 Unity，避免 Exit 时子进程被一并杀掉。
        /// </summary>
        static bool TryLaunchDetachedRestartHelper(string unityEditorPath, string projectPath, out string error)
        {
            error = null;

            try
            {
                if (Application.platform == RuntimePlatform.WindowsEditor)
                    return TryLaunchWindowsRestartHelper(unityEditorPath, projectPath, out error);

                // macOS / Linux：shell 延迟启动
                return TryLaunchUnixRestartHelper(unityEditorPath, projectPath, out error);
            }
            catch (System.Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        static bool TryLaunchWindowsRestartHelper(string unityEditorPath, string projectPath, out string error)
        {
            error = null;

            var batchPath = Path.Combine(
                Path.GetTempPath(),
                $"UnityProjectRestart_{Process.GetCurrentProcess().Id}.bat");

            var sb = new StringBuilder();
            sb.AppendLine("@echo off");
            sb.AppendLine("chcp 65001 >nul");
            sb.AppendLine($"timeout /t {RestartDelaySeconds} /nobreak >nul");
            sb.AppendLine($"start \"\" \"{unityEditorPath}\" -projectPath \"{projectPath}\"");
            sb.AppendLine($"del \"{batchPath}\"");

            File.WriteAllText(batchPath, sb.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C \"\"{batchPath}\"\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                error = "无法启动 cmd 重启助手。";
                return false;
            }

            return true;
        }

        static bool TryLaunchUnixRestartHelper(string unityEditorPath, string projectPath, out string error)
        {
            error = null;

            var scriptPath = Path.Combine(
                Path.GetTempPath(),
                $"UnityProjectRestart_{Process.GetCurrentProcess().Id}.sh");

            var sb = new StringBuilder();
            sb.AppendLine("#!/bin/bash");
            sb.AppendLine($"sleep {RestartDelaySeconds}");
            sb.AppendLine($"\"{unityEditorPath}\" -projectPath \"{projectPath}\" &");
            sb.AppendLine($"rm -f \"{scriptPath}\"");

            File.WriteAllText(scriptPath, sb.ToString());
            chmod(scriptPath);

            var startInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = scriptPath,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                error = "无法启动 shell 重启助手。";
                return false;
            }

            return true;
        }

        static void chmod(string path)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "chmod",
                    Arguments = $"+x \"{path}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                })?.WaitForExit(1000);
            }
            catch
            {
                // 忽略 chmod 失败
            }
        }
    }
}
