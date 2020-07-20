using System.IO;
using UnityEngine;

namespace CherryHill.Utility
{
    public class PathUtillity
    {
        static string[] patterns = { "/", "Assets", "Script" };
        static string slotDirName = @"\Slot\";

        public static string GetParentStringPath()
        {
            string parentPath = "";
            string[] words = Application.dataPath.Split('/');

            for (int i = 0; i < words.Length - 2; i++)
            {
                parentPath += string.Concat(words[i], "/");
            }

            return parentPath;
        }

        public static string GetFileProjectPath(FileInfo file)
        {
            string[] words = file.FullName.Split('\\');
            string projectPath = string.Empty;

            for (int i = words.Length - 2; i >= 0; i--)
            {
                if (words[i] == ("Assets"))
                {
                    break;
                }
                projectPath = string.Concat(patterns[0], words[i]) + projectPath;
            }

            return projectPath;
        }

        public static bool IsSlotDirectory(DirectoryInfo info)
        {
            if (info.FullName.Contains(slotDirName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}