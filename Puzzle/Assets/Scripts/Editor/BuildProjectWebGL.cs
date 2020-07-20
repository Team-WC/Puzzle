//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor.Callbacks;
//using UnityEditor;
//using System;
//using System.IO;
//using System.Threading.Tasks;
//using System.Diagnostics;
//using Debug = UnityEngine.Debug;

//public enum BuildVersion
//{
//    DEV = 0,
//    ALPHA,
//    LIVE,
//}

//public class BuildProjectAndroid
//{
//    public static bool buildCompleted = true;
//    static bool warning = false;
//    static string PCDeviceSymbol = "PC";
//    static string AndroidDeviceSymbol = "Android";
//    static string DesktopPath = "C:/Users/seungho/Desktop/";

//    static string[] jsonDirPattern = { "Build", "/", "GameLoader.json" };

//    static string projectName = "GameLoader";

//    static Action act;

//    static BuildVersion buildVersion = BuildVersion.DEV;

//    #region dev

//    static string devServerVersion = "DEV";
//    static string devDirectory = "GameLoaderDEV";

//    #endregion

//    #region alpha

//    static string alphaServerVersion = "ALPHA";
//    static string alphaDirectory = "GameLoaderALPHA";
//    //static string alphaDevice = "PC";

//    #endregion

//    #region live

//    static string liveServerVersion = "LIVE";
//    static string liveDirectory = "GameLoaderLIVE";
//    //static string liveDevice = "PC";

//    #endregion

//    static string symbolPattern = ";";

//    public static void SetCallbackAndStart(Queue<Action> afterActions)
//    {
//        act = afterActions.Dequeue();

//        while (!buildCompleted)
//        {

//        }

//        act?.Invoke();
//    }

//    public static void BuildStart(BuildVersion version)
//    {
//        buildCompleted = false;
//        buildVersion = version;

//        Debug.Log("BuildStart " + buildVersion.ToString());

//        ProjectSetting(buildVersion);
//        Build();
//    }

//    public static void ProjectSetting(BuildVersion version)
//    {
//        PlayerSettings.companyName = "CHERRYHILL";
//        PlayerSettings.productName = "CHERRYHILL";
//        PlayerSettings.bundleVersion = "1.0";
//        PlayerSettings.colorSpace = ColorSpace.Gamma;

//        switch (version)
//        {
//            case BuildVersion.DEV:
//                PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
//                PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.ScriptOnly);
//                PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
//                PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
//                PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
//                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, SetBuildSymbolByServerVersion(BuildVersion.DEV));
//                break;
//            case BuildVersion.ALPHA:
//                PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
//                PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.ScriptOnly);
//                PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
//                PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
//                PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
//                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, SetBuildSymbolByServerVersion(BuildVersion.ALPHA));
//                break;
//            case BuildVersion.LIVE:
//                PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
//                PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.None);
//                PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
//                PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
//                PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.None);
//                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, SetBuildSymbolByServerVersion(BuildVersion.LIVE));
//                break;
//        }
//    }

//    static string SetBuildSymbolByServerVersion(BuildVersion version)
//    {
//        string symbolStr = "";

//        switch (version)
//        {
//            case BuildVersion.DEV:
//                symbolStr = string.Concat(devServerVersion, symbolPattern, AndroidDeviceSymbol);
//                break;
//            case BuildVersion.ALPHA:
//                symbolStr = string.Concat(alphaServerVersion, symbolPattern, AndroidDeviceSymbol);
//                break;
//            case BuildVersion.LIVE:
//                symbolStr = string.Concat(liveServerVersion, symbolPattern, AndroidDeviceSymbol);
//                break;
//        }

//        return symbolStr;
//    }

//    static void Build()
//    {
//        var scenes = new string[] { "Assets/Scene/0_Loader.unity", "Assets/Scene/1_Lobby.unity", "Assets/Scene/2_Seat.unity", "Assets/Scene/3_Game.unity", "Assets/Scene/4_Lounge.unity" };
//        var flags = BuildOptions.None;
//        BuildPipeline.BuildPlayer(scenes, "GameLoader", BuildTarget.WebGL, flags);
//    }


//    [PostProcessBuild(1)]
//    public static void Run(BuildTarget target, string buildPath)
//    {
//#if !ANDROID
//        MoveBuildDirectory(buildPath);


//        SetDefaultSymbolByServerVersion(BuildVersion.DEV);
      
//        Debug.Log("PostProcess Complete!");
//        buildCompleted = true;

//#endif
//    }

//    public static void SetDefaultSymbolByServerVersion(BuildVersion version)
//    {
//        string symbolStr = "";

//        switch (version)
//        {
//            case BuildVersion.DEV:
//                symbolStr = string.Concat(devServerVersion, symbolPattern, PCDeviceSymbol);
//                break;
//            case BuildVersion.ALPHA:
//                symbolStr = string.Concat(alphaServerVersion, symbolPattern, PCDeviceSymbol);
//                break;
//            case BuildVersion.LIVE:
//                symbolStr = string.Concat(liveServerVersion, symbolPattern, PCDeviceSymbol);
//                break;
//        }

//        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbolStr);
//        //return symbolStr;
//    }

//    static bool MoveBuildDirectory(string sourcePath)
//    {
//        string path = "";
//        DesktopPath = new DirectoryInfo(Application.dataPath).Parent.Parent.FullName + "/GameLoaderBulid";//Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

//        switch (buildVersion)
//        {
//            case BuildVersion.DEV:
//                path = string.Concat(DesktopPath, "/", devDirectory, "/", projectName, "/");
//                break;
//            case BuildVersion.ALPHA:
//                path = string.Concat(DesktopPath, "/", alphaDirectory, "/", projectName, "/");
//                break;
//            case BuildVersion.LIVE:
//                path = string.Concat(DesktopPath, "/", liveDirectory, "/", projectName, "/");
//                break;
//        }

//        DirectoryInfo di = new DirectoryInfo(path);

//        if (!di.Parent.Exists)
//        {
//            di.Parent.Create();
//        }

//        if (di.Exists)
//        {
//            di.Delete(true);
//        }

//        FileUtil.CopyFileOrDirectory(sourcePath, path);

//        ChangeJsonData(string.Concat(path, jsonDirPattern[1], jsonDirPattern[0], jsonDirPattern[1], jsonDirPattern[2]));

//        return true;
//    }

//    //[MenuItem("Tools/TestJson")]
//    static void test()
//    {
//        //ChangeJsonData(@"C:\Users\seungho\Desktop\GameLoaderDEV\Build\BuildTest.Json");
//        DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

//        string path = "";

//        switch (buildVersion)
//        {
//            case BuildVersion.DEV:
//                path = string.Concat(DesktopPath, "/", devDirectory, "/", projectName, "/");
//                break;
//            case BuildVersion.ALPHA:
//                path = string.Concat(DesktopPath, "/", alphaDirectory, "/", projectName, "/");
//                break;
//            case BuildVersion.LIVE:
//                path = string.Concat(DesktopPath, "/", liveDirectory, "/", projectName, "/");
//                break;
//        }

//        path = path.Replace('\\', '/');

//        Debug.Log(path);
//    }

//    static void ChangeJsonData(string path)
//    {
//        Debug.Log("Change Json " + path);
//        string newJson = "";

//        using (StreamReader reader = new StreamReader(path))
//        {
//            string source = reader.ReadLine();

//            newJson += source;

//            string[] values;
//            int lineNumb = 1;

//            while (source != null)
//            {
//                values = source.Split(',');

//                if (values.Length == 0)
//                {
//                    reader.Close();

//                    return;
//                }
//                source = reader.ReadLine();
//                lineNumb++;

//                //Debug.Log(source + ", " + lineNumb);

//                if (!string.IsNullOrEmpty(source))
//                {
//                    if (source.Contains("backgroundColor"))
//                    {
//                        continue;
//                    }

//                    newJson += source;
//                }

//                //File.WriteAllText(path, data);
//            }
//            Debug.Log(newJson);
//            reader.Dispose();
//        }

//        //File.Delete(path);

//        //File.CreateText(path);
//        File.WriteAllText(path, newJson);
//    }

//    [MenuItem("Tools/BatTest")]
//    private static void ProcessRun()
//    {
//        ProcessStartInfo psi = new ProcessStartInfo();

//        psi.FileName = "cmd.exe";
//        psi.Arguments = "/C filemove.bat";

//        psi.RedirectStandardOutput = true;
//        psi.UseShellExecute = false;

//        Process proc = Process.Start(psi);

//        while (true)
//        {
//            string txt = proc.StandardOutput.ReadLine(); // blocking 함수

//            if (txt == null) // 프로세스가 종료한 경우 null 반환
//            {
//                break;
//            }

//            Debug.Log(txt);
//            Console.WriteLine(txt);
//        }
//    }

//    static void ShowErrorlog(Exception exception)
//    {
//        Debug.LogError(exception.Message);
//    }
//}




