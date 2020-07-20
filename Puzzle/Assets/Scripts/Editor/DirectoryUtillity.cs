using System.IO;
using UnityEditor;
using UnityEngine;

namespace CherryHill.Utility
{
    public class DirectoryUtillity
    {
        static string sourcetPath = Application.dataPath + @"/Script";
        static string[] patterns = { "/", "Assets", "Script" };

        static string targetProject = string.Empty;

        static bool warning = false;

        public static void CopyDirectory()
        {
            string parentPath = "";
            string[] words = Application.dataPath.Split('/');

            warning = false;

            for (int i = 0; i < words.Length - 2; i++)
            {
                parentPath += string.Concat(words[i], patterns[0]);
            }

            DirectoryInfo di = new DirectoryInfo(Application.dataPath).Parent.Parent;

            string destPath = string.Concat(EditorUtility.OpenFolderPanel("Select target folder", di.FullName, "OK"), patterns[0], patterns[1], patterns[0], patterns[2]); //string.Concat(project.FullName, patterns[0], patterns[1], patterns[0], patterns[2]);

            DirectoryInfo dInfo = new DirectoryInfo(destPath);

            if (destPath.Length > 0 && dInfo.Exists)
            {
                if (!warning)
                {
                    warning = EditorUtility.DisplayDialog("Warning", "대상 프로젝트 폴더의 Assets/Scripts/에 중복되는 스크립트들을 덮어쓰기 합니다. 올바른 프로젝트 폴더를 선택했는지 확인해주세요.", "OK", "No");
                }

                if (warning)
                {
                    Debug.Log("Copy Directory " + sourcetPath + " to " + destPath);

                    CopyDirectory(sourcetPath, destPath, true);

                    Debug.Log("Copy Complete");
                }
            }
        }

        public static void CopyScript(string destName)
        {
            warning = false;
            CherryHillUtillityWindow.checkfiles.Clear();

            string[] words = destName.Split('/');
            targetProject = words[words.Length - 1];

            string destPath = string.Concat(destName, patterns[0], patterns[1], patterns[0], patterns[2]); //string.Concat(project.FullName, patterns[0], patterns[1], patterns[0], patterns[2]);

            DirectoryInfo dInfo = new DirectoryInfo(destPath);

            Debug.Log(destPath + ", " + dInfo.Exists);

            if (destPath.Length > 0 && dInfo.Exists)
            {
                if (!warning)
                {
                    warning = EditorUtility.DisplayDialog("Warning", words[words.Length - 1] + "프로젝트의 Assets/Scripts/에 스크립트들을 복사 합니다. 슬롯 클래스는 덮어쓰기만, 이외의 클래스 들은 덮어쓰기 및 복사가 됩니다. 올바른 프로젝트 폴더를 선택했는지 확인해주세요.", "OK", "No");
                }

                if (warning)
                {
                    Debug.Log("Copy Directory " + sourcetPath + " to " + destPath);

                    CopyDirectory(sourcetPath, destPath, true);

                    Debug.Log("Copy Complete. Check delete file");

                    for (int i = 0; i < CherryHillUtillityWindow.checkfiles.Count; i++)
                    {
                        FileUtillity.RemoveDiffPathScript(targetProject, CherryHillUtillityWindow.checkfiles[i]);
                    }

                    CherryHillUtillityWindow.checkfiles.Clear();

                    Debug.Log("File Delete Complete");
                }
            }
        }

        public static void CopyScript(string[] projects, string parentPath, bool isSlot)
        {
            warning = false;
            CherryHillUtillityWindow.checkfiles.Clear();

            if (!warning)
            {
                string warnMsg = isSlot ? "슬롯" : "로비";
                warning = EditorUtility.DisplayDialog("Warning", "모든 " + warnMsg + " 프로젝트의 Assets/Scripts/에 현재 프로젝트의 스크립트들을 복사 합니다. 슬롯 클래스는 덮어쓰기만, 이외의 클래스 들은 덮어쓰기 및 복사가 됩니다. 올바른 프로젝트 폴더를 선택했는지 확인해주세요.", "OK", "No");
            }

            if (warning)
            {
                for (int i = 0; i < projects.Length; i++)
                {
                    if (Application.dataPath.Contains(projects[i]))
                    {
                        continue;
                    }

                    string destDataPath = string.Concat(parentPath, "Android_Project/", projects[i]);
                    targetProject = projects[i];
                    //string[] words = destDataPath.Split('/');
                    //Debug.Log(parentPath + ", " + projects[i]);

                    string destPath = string.Concat(destDataPath, patterns[0], patterns[1], patterns[0], patterns[2]); //string.Concat(project.FullName, patterns[0], patterns[1], patterns[0], patterns[2]);

                    DirectoryInfo dInfo = new DirectoryInfo(destPath);

                    if (destPath.Length > 0 && dInfo.Exists)
                    {
                        Debug.Log("Copy " + sourcetPath + " to " + destPath);

                        CopyDirectory(sourcetPath, destPath, true);
                    }

                    Debug.Log("Check delete file");

                    for (int j = 0; j < CherryHillUtillityWindow.checkfiles.Count; j++)
                    {
                        FileUtillity.RemoveDiffPathScript(targetProject, CherryHillUtillityWindow.checkfiles[j]);
                    }

                    CherryHillUtillityWindow.checkfiles.Clear();

                    Debug.Log("File Delete Complete");
                }

                Debug.Log("Copy Complete");
            }
        }

        private static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            //Debug.Log(destDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "다음 파일을 찾을 수 없습니다 : " + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName) && !PathUtillity.IsSlotDirectory(dir))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileUtillity.CopyAllFileInDirectory(dir, destDirName);

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}

