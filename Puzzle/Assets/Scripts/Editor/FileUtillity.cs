using System.IO;
using UnityEngine;

namespace CherryHill.Utility
{
    public class FileUtillity
    {
        static string[] patterns = { "/", "Assets", "Script" };

        // <summary>
        /// 테스트 후 개선? 예외사항 고려?
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        public static void CopyAllFileInDirectory(DirectoryInfo dir, string destDirName)
        {
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(".meta")) continue;

                string temppath = Path.Combine(destDirName, file.Name);

                if (PathUtillity.IsSlotDirectory(dir))
                {
                    if (File.Exists(temppath))
                    {
                        file.CopyTo(temppath, true);
                    }
                }
                else
                {
                    CherryHillUtillityWindow.checkfiles.Add(file);

                    FileInfo copyedFile = file.CopyTo(temppath, true);
                }
            }
        }

        public static void RemoveDiffPathScript(string projectName, FileInfo source)
        {
            string targetPath = string.Concat(PathUtillity.GetParentStringPath(), projectName, patterns[0], patterns[1], patterns[0], patterns[2]);

            DirectoryInfo di = new DirectoryInfo(targetPath);

            if (di.Exists)
            {
                FileInfo[] fInfos = di.GetFiles(source.Name, SearchOption.AllDirectories);

                foreach (var file in fInfos)
                {
                    if (PathUtillity.GetFileProjectPath(source) != PathUtillity.GetFileProjectPath(file))
                    {
                        file.Delete();
                    }
                }
            }
        }

        private static void Run(string sourcePath, string destPath)
        {
            FileInfo file = new FileInfo(sourcePath);

            if (!file.Exists)
            {
                throw new FileNotFoundException(
                    "다음 파일을 찾을 수 없습니다 : " + sourcePath);
            }

            string temppath = Path.Combine(destPath, file.Name);
            Debug.Log("Copy Process :: " + temppath);

            file.CopyTo(temppath, true);
        }
    }
}
