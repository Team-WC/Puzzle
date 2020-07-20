using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CherryHill.Utility.EditorImprovements
{
    public class EditorUtilities
    {
        [MenuItem("Tools/Hot keys/Clear Console &c")]
        static void ClearConsole()
        {
            var logEntries = Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }

        [MenuItem("Tools/Hot keys/Toggle Inspector Lock (shortcut) &e")]
        public static void SelectLockableInspector()
        {
            EditorWindow inspectorToBeLocked = EditorWindow.mouseOverWindow;
            if (inspectorToBeLocked != null && inspectorToBeLocked.GetType().Name == "InspectorWindow")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
                PropertyInfo propertyInfo = type.GetProperty("isLocked");
                bool value = (bool)propertyInfo.GetValue(inspectorToBeLocked, null);
                propertyInfo.SetValue(inspectorToBeLocked, !value, null);

                inspectorToBeLocked.Repaint();
            }
        }

        public static bool GetIsLocked()
        {
            EditorWindow inspectorToBeLocked = EditorWindow.focusedWindow;
            if (inspectorToBeLocked != null && inspectorToBeLocked.GetType().Name == "InspectorWindow")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
                PropertyInfo propertyInfo = type.GetProperty("isLocked");
                bool value = (bool)propertyInfo.GetValue(inspectorToBeLocked, null);
                //propertyInfo.SetValue(inspectorToBeLocked, !value, null);

                //inspectorToBeLocked.Repaint();
                return value;
            }

            //Debug.LogError("inspector not focussed!");

            return false;
        }

        //[MenuItem("Tools/Hot keys/Toggle Inspector Mode &d")]
        static void ToggleInspectorDebug()
        {
            EditorWindow targetInspector = EditorWindow.mouseOverWindow;
            if (targetInspector != null && targetInspector.GetType().Name == "InspectorWindow")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
                FieldInfo field = type.GetField("m_InspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

                InspectorMode mode = (InspectorMode)field.GetValue(targetInspector);                               
                mode = (mode == InspectorMode.Normal ? InspectorMode.Debug : InspectorMode.Normal);                
                                                                                                                   

                MethodInfo method = type.GetMethod("SetMode", BindingFlags.NonPublic | BindingFlags.Instance);     
                method.Invoke(targetInspector, new object[] { mode });                                             

                targetInspector.Repaint();
            }
        }

        //[MenuItem("Tools/Hot keys/DevSetting &d")]
        //static void ChangeDevSetting()
        //{
        //    BuildProjectAndroid.SetDefaultSymbolByServerVersion(BuildVersion.DEV);
        //}

        //[MenuItem("Tools/Hot keys/AlphaSetting &a")]
        //static void ChangeAlphaSetting()
        //{
        //    BuildProjectAndroid.SetDefaultSymbolByServerVersion(BuildVersion.ALPHA);
        //}

        //[MenuItem("Tools/Hot keys/LiveSetting &l")]
        //static void ChangeLiveSetting()
        //{
        //    BuildProjectAndroid.SetDefaultSymbolByServerVersion(BuildVersion.LIVE);
        //}
    }
}

