using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace CherryHill.Utility
{
    public enum Phase { Setup, Processing, Complete };

    public class CherryHillUtillityWindow : EditorWindow
    {
        public static List<FileInfo> checkfiles = new List<FileInfo>();
        private const float PLAY_MODE_REFRESH_INTERVAL = 1f;

        private static Texture2D tex;

        private Phase currentPhase = Phase.Setup;

        private double nextPlayModeRefreshTime = 0f;

        private Vector2 scrollPosition;

        private string errorMessage = string.Empty;

        private bool restoreInitialSceneSetup = true;

        private bool copyScriptToggled = true;

        private bool lobbyMenuToggled = false;

        private bool slotMenuToggled = false;

        private bool uiRaycastOffToggled = true;

        private bool projectSettingMenuOpened = false;

        private bool isDevSetting = false;
        private bool isAlphaSetting = false;
        private bool isLiveSetting = false;

        private bool buildMenuOpened = false;

        private bool isDevBuild = false;
        private bool isAlphaBuild = false;
        private bool isLiveBuild = false;

        private AnimationClip clip;

        private string[] toggleStr = { "Open", "Close" };

        Color windowColor = new Color(.2f, .2f, .2f, 1);

        public static readonly GUILayoutOption GL_EXPAND_WIDTH = GUILayout.ExpandWidth(true);
        public static readonly GUILayoutOption GL_EXPAND_HEIGHT = GUILayout.ExpandHeight(true);
        public static readonly GUILayoutOption GL_WIDTH_25 = GUILayout.Width(25);
        public static readonly GUILayoutOption GL_WIDTH_100 = GUILayout.Width(100);
        public static readonly GUILayoutOption GL_WIDTH_250 = GUILayout.Width(250);
        
        public static readonly GUILayoutOption GL_HEIGHT_30 = GUILayout.Height(30);
        public static readonly GUILayoutOption GL_HEIGHT_35 = GUILayout.Height(35);
        public static readonly GUILayoutOption GL_HEIGHT_40 = GUILayout.Height(40);

        private string[] lobbyProjectStrs = { "GameLobby_Android", "NetworkJackpot_2_Android", "SlotUI_Android", "BigWin_Android", "Toast_Android" };
        private string[] slotProjectStrs = {
            "GameofGems_Android",
            "CaptainsLog_Android",
            "BloodedFangs_Android",
            "GoldenTail_Android",
            "FiestaDeMayo_Android",
            "WinterGuardian_Android",
            "LostGoldCave_Android",
            "FatalCherries_Android",
            "Bison_Android",
            "Hercules_Android",
            "LuckyClover_Android",
            "EgyptianCosmos_Android",
            "LeSacre_Android",
            "FortuneHalloween_Android",
            "DiamondCity_Android",
            "TwoLegends_Android",
            "ShinySeven_Android",
            "SevenElements_Android",
            "AmazingRespin_Android",
            "ThunderRespin_Android",
            "Pandaboo_Android",
            "MidnightCircus_Android",
            "SweetDream_Android",
            "ExtremeSeven_Android",
            "ExtreamSevenDeluxe_Android",
            "PerfectSheriff_Android",
            "BestFriends_Android",
            "GongXiFaDaCai_Android",
        };

        [MenuItem("Window/CherryHill Utility")]
        private static void Init()
        {
            tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, new Color(0.25f, 0.4f, 0.25f));
            tex.Apply();

            CherryHillUtillityWindow window = GetWindow<CherryHillUtillityWindow>();
            window.titleContent = new GUIContent("CherryHill Utility Pannel");
            window.minSize = new Vector2(250, 220f);
            
            window.Show();
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void Update()
        {
            // Refresh the window at a regular interval in play mode to update the tooltip
            if (EditorApplication.isPlaying && currentPhase == Phase.Complete && EditorApplication.timeSinceStartup >= nextPlayModeRefreshTime)
            {
                nextPlayModeRefreshTime = EditorApplication.timeSinceStartup + PLAY_MODE_REFRESH_INTERVAL; ;
                Repaint();
            }
        }

        private void OnGUI()
        {
            //scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GL_EXPAND_WIDTH, GL_EXPAND_HEIGHT);
            EditorGUI.DrawRect(new Rect(0, 0, Screen.width, Screen.height), windowColor);

            GUILayout.BeginVertical();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            GUILayout.Space(10);

            if (errorMessage.Length > 0)
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);

            GUILayout.Space(10);

            //GUILayout.Box("Copy Script", GL_EXPAND_WIDTH);

            GUILayout.BeginVertical();

            copyScriptToggled = EditorGUILayout.ToggleLeft("CopyScript Menu", copyScriptToggled);

            if (copyScriptToggled)
            {
                ExposeCopyScriptMenu();
            }

            GUILayout.Space(10);

            uiRaycastOffToggled = EditorGUILayout.ToggleLeft("UI Menu", uiRaycastOffToggled);

            if (uiRaycastOffToggled)
            {
                ExposeUIRayCastSetMenu();

                GUILayout.Space(10);

                using (new BackGroundColorScope(Color.yellow))
                {
                    if (GUILayout.Button("Generate UI Object"))
                    {
                        GeneratorUIObject();
                    }
                }
            }

            GUILayout.Space(10);

            projectSettingMenuOpened = EditorGUILayout.ToggleLeft("프로젝트 세팅", projectSettingMenuOpened);

            if (projectSettingMenuOpened)
            {
                ExposeProjectSettingMenu();
            }

            GUILayout.Space(10);

            buildMenuOpened = EditorGUILayout.ToggleLeft("Build Menu", buildMenuOpened);

            if (buildMenuOpened)
            {
                ExposeBuildMenu();
            }

            GUILayout.Space(10);

            using (new BackGroundColorScope(Color.blue))
            {
                if (GUILayout.Button("파티클 에셋 저장"))
                {
                    SaveParticleTextureInFolder(Selection.activeGameObject);
                }
            }

            GUILayout.Space(10);

            using (new BackGroundColorScope(Color.blue))
            {
                if (GUILayout.Button("슬롯 생성"))
                {
                    CreateSlotObject();
                }
            }

            GUILayout.Space(10);
            
            GUILayout.EndVertical();

            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        #region Copy Util GUI

        private void ExposeCopyScriptMenu()
        {
            var boxstyle = new GUIStyle(GUI.skin.box);
            
            boxstyle.normal.textColor = Color.green;

            using (new BackGroundColorScope(Color.black))
            {
                GUILayout.Box("복사할 프로젝트 폴더를 선택하세요.", boxstyle, GUILayout.ExpandWidth(true));
            }

            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            string lobbyMenuStr = lobbyMenuToggled ? "Lobby Menu " + toggleStr[1] : "Lobby Menu " + toggleStr[0];

            lobbyMenuToggled = EditorGUILayout.ToggleLeft(lobbyMenuStr, lobbyMenuToggled);

            GUILayout.EndHorizontal();

            if (lobbyMenuToggled)
            {
                ShowLobbyEditorButtons();
            }

            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            string slotMenuStr = slotMenuToggled ? "Slot Menu " + toggleStr[1] : "Slot Menu " + toggleStr[0];

            slotMenuToggled = EditorGUILayout.ToggleLeft(slotMenuStr, slotMenuToggled);

            GUILayout.EndHorizontal();

            if (slotMenuToggled)
            {
                ShowSlotEditorButtons();
            }
        }

        private void ShowLobbyEditorButtons()
        {
            GUILayout.BeginHorizontal();
            string parentPath = GetParentStringPath();
            string destPath = "";
            //string[] words = Application.dataPath.Split('/');

            using (new BackGroundColorScope(Color.green))
            {
                for (int i = 0; i < lobbyProjectStrs.Length; i++)
                {
                    if (Application.dataPath.Contains(lobbyProjectStrs[i]))
                    {
                        continue;
                    }

                    if (GUILayout.Button(lobbyProjectStrs[i]))
                    {
                        destPath = string.Concat(parentPath, "Android_Project/", lobbyProjectStrs[i]);
                        //Debug.Log(parentPath);
                        DirectoryUtillity.CopyScript(destPath);
                    }
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            using (new BackGroundColorScope(Color.white))
            {
                if (GUILayout.Button("All Lobby"))
                {
                    DirectoryUtillity.CopyScript(lobbyProjectStrs, parentPath, false);
                    //for (int i = 0; i < lobbyProjectStrs.Length; i++)
                    //{
                    //    destPath = string.Concat(parentPath, lobbyProjectStrs[i]);
                    //    //Debug.Log(parentPath);
                    //    CopyScripts.CopyScript(destPath);
                    //}  
                }
            }

            GUILayout.EndHorizontal();
        }

        private void ShowSlotEditorButtons()
        {
            GUILayout.BeginVertical();

            string destPath = "";
            string parentPath = GetParentStringPath();

            using (new BackGroundColorScope(Color.red))
            {
                bool isVerticaled = true;

                for (int i = 0; i < slotProjectStrs.Length; i++)
                {
                    if (Application.dataPath.Contains(slotProjectStrs[i]))
                    {
                        continue;
                    }

                    if( i % 3 == 0)
                    {
                        if (isVerticaled)
                        {
                            GUILayout.BeginHorizontal();

                            isVerticaled = false;
                        }
                    }

                    if (GUILayout.Button(slotProjectStrs[i]))
                    {
                        destPath = string.Concat(parentPath, "Android_Project/", slotProjectStrs[i]);
                        //Debug.Log(parentPath);
                        DirectoryUtillity.CopyScript(destPath);
                    }

                    if ( i % 3 == 2)
                    {
                        if (!isVerticaled)
                        {
                            GUILayout.EndHorizontal();
                            isVerticaled = true;
                        }
                    }
                }
            }

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            using (new BackGroundColorScope(Color.white))
            {
                if (GUILayout.Button("All Slot"))
                {
                    DirectoryUtillity.CopyScript(slotProjectStrs, parentPath, true);
                    //for (int i = 0; i < slotProjectStrs.Length; i++)
                    //{
                    //    destPath = string.Concat(parentPath, slotProjectStrs[i]);
                    //    //Debug.Log(parentPath);
                    //    CopyScripts.CopyScript(destPath);
                    //}
                }
            }

            GUILayout.EndHorizontal();
        }

        private string GetParentStringPath()
        {
            string parentPath = "";
            string[] words = Application.dataPath.Split('/');


            for (int i = 0; i < words.Length - 2; i++)
            {
                parentPath += string.Concat(words[i], "/");
            }

            return parentPath;
        }

        #endregion

        #region UIRaycast GUI

        private void ExposeUIRayCastSetMenu()
        {
            var boxstyle = new GUIStyle(GUI.skin.box);

            boxstyle.normal.textColor = Color.cyan;

            using (new BackGroundColorScope(Color.black))
            {
                GUILayout.Box("Hierachy의 게임 오브젝트를 선택 한 후, 버튼을 누르면 해당 오브젝트를 포함한 자식 오브젝트의 UI Image, Text들의 Raycast옵션이 Off됩니다.",
                boxstyle, GUILayout.ExpandHeight(false));
            }

            var style = new GUIStyle(GUI.skin.button);

            //style.normal.textColor = Color.red;

            using (new BackGroundColorScope(Color.cyan))
            {
                if (GUILayout.Button("Raycast Off", style))
                {
                    UIRayCastOff.OffRayCastExceptButton();
                }
            }
        }

        #endregion


        #region ProjectSetting Menu

        private void ExposeProjectSettingMenu()
        {
            //GUILayout.BeginHorizontal();

            //var btnStyle = new GUIStyle(GUI.skin.toggle);

            //btnStyle.normal.textColor = Color.white;

            //using (new BackGroundColorScope(Color.magenta))
            //{
            //    GUILayout.Space(10);

            //    if (GUILayout.Button("Dev"))
            //    {
            //        BuildProjectAndroid.SetDefaultSymbolByServerVersion(BuildVersion.DEV);
            //    }

            //    if (GUILayout.Button("Alpha"))
            //    {
            //        BuildProjectAndroid.SetDefaultSymbolByServerVersion(BuildVersion.ALPHA);
            //    }

            //    if (GUILayout.Button("Live"))
            //    {
            //        BuildProjectAndroid.SetDefaultSymbolByServerVersion(BuildVersion.LIVE);
            //    }
            //}

            //GUILayout.EndHorizontal();
        }

        #endregion

        #region Build GUI

        private void ExposeBuildMenu()
        {
            var boxstyle = new GUIStyle(GUI.skin.box);

            boxstyle.normal.textColor = Color.magenta;

            GUILayout.Space(10);

            using (new BackGroundColorScope(Color.grey))
            {
                GUILayout.Box("Build할 버전을 선택 해 주세요.",
                boxstyle, GUILayout.ExpandHeight(false));
            }

            var toggleStyle = new GUIStyle(GUI.skin.toggle);

            toggleStyle.normal.textColor = Color.white;

            //using (new BackGroundColorScope(Color.magenta))
            //{
            //    GUILayout.Space(10);

            //    isDevBuild = EditorGUILayout.ToggleLeft("Dev", isDevBuild);

            //    GUILayout.Space(10);

            //    isAlphaBuild = EditorGUILayout.ToggleLeft("Alpha", isAlphaBuild);

            //    GUILayout.Space(10);

            //    isLiveBuild = EditorGUILayout.ToggleLeft("Live", isLiveBuild);
                //GUILayout.Box("Hierachy의 게임 오브젝트를 선택 한 후, 버튼을 누르면 해당 오브젝트를 포함한 자식 오브젝트의 UI Image, Text들의 Raycast옵션이 Off됩니다.",
                //boxstyle, GUILayout.ExpandHeight(false));

                //GUILayout.Space(10);

                //GUI.enabled = isDevBuild || isAlphaBuild || isLiveBuild;

                //if (GUILayout.Button("Build"))
                //{
                //    Queue<Action> queue = new Queue<Action>();

                //    if (isDevBuild)
                //    {
                //        //queue.Enqueue(() => {
                //        //    BuildProjectWebGL.BuildStart(BuildVersion.DEV);
                //        //});
                //        BuildProjectAndroid.BuildStart(BuildVersion.DEV);
                //    }

                //    if (isAlphaBuild)
                //    {
                //        //queue.Enqueue(() => {
                //        //    BuildProjectWebGL.BuildStart(BuildVersion.ALPHA);
                //        //});
                //        BuildProjectAndroid.BuildStart(BuildVersion.ALPHA);
                //    }

                //    if (isLiveBuild)
                //    {
                //        //queue.Enqueue(() => {
                //        //    BuildProjectWebGL.BuildStart(BuildVersion.LIVE);
                //        //});
                //        BuildProjectAndroid.BuildStart(BuildVersion.LIVE);
                //    }

                //    //BuildProjectWebGL.SetCallbackAndStart(queue);
                //}

            //    GUI.enabled = true;
            //}
        }

        //IEnumerator 

        #endregion

        private void GeneratorUIObject()
        {
            if(Selection.objects == null || Selection.objects.Length == 0)
            {
                Debug.LogError("선택된 이미지가 없습니다.");
                return;
            }

            Transform parent = FindObjectOfType<Canvas>().transform.GetChild(0);
            GameObject go = new GameObject("UIParent");
            RectTransform rectTR = go.AddComponent<RectTransform>();
            rectTR.sizeDelta = Vector2.one;
            rectTR.anchoredPosition = Vector2.zero;
            go.transform.SetParent(parent);

            Dictionary<string, ButtonUtility> swapBtndic = new Dictionary<string, ButtonUtility>();

            try
            {
                Sprite[] selSprites = Selection.GetFiltered<Sprite>(SelectionMode.Unfiltered);

                foreach (var sprite in selSprites)
                {
                    if (sprite.name.ToLower().Contains("btn"))
                    {
                        if (sprite.name.Contains("Normal"))
                        {
                            string key = sprite.name.Replace("Normal", "");

                            if (!swapBtndic.ContainsKey(key))
                            {
                                ButtonUtility buttonUtility = new ButtonUtility();
                                buttonUtility.Normal = sprite;

                                swapBtndic.Add(key, buttonUtility);
                            }
                            else
                            {
                                swapBtndic[key].Normal = sprite;
                            }
                        }
                        else if (sprite.name.Contains("Over"))
                        {
                            string key = sprite.name.Replace("Over", "");

                            if (!swapBtndic.ContainsKey(key))
                            {
                                ButtonUtility buttonUtility = new ButtonUtility();
                                buttonUtility.Over = sprite;

                                swapBtndic.Add(key, buttonUtility);
                            }
                            else
                            {
                                swapBtndic[key].Over = sprite;
                            }
                        }
                        else if(sprite.name.Contains("Click"))
                        {
                            string key = sprite.name.Replace("Click", "");

                            if (!swapBtndic.ContainsKey(key))
                            {
                                ButtonUtility buttonUtility = new ButtonUtility();
                                buttonUtility.Click = sprite;

                                swapBtndic.Add(key, buttonUtility);
                            }
                            else
                            {
                                swapBtndic[key].Click = sprite;
                            }
                        }
                    }
                    else
                    {
                        GameObject g = new GameObject(sprite.name);
                        g.transform.SetParent(go.transform);
                        UnityEngine.UI.Image img = g.AddComponent<UnityEngine.UI.Image>();

                        img.sprite = sprite;
                        img.SetNativeSize();
                    }
                }

                if(swapBtndic.Count > 0)
                {
                    foreach(var btn in swapBtndic)
                    {
                        GenerateButtonObject(btn.Value.Normal, btn.Value.Click, btn.Value.Over, go.transform);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Destroy(go);
            }
            finally
            {
                UIRayCastOff.RayCastOffTargetsChilds(go.transform);
            }
        }

        private void GenerateButtonObject(Sprite normal, Sprite click, Sprite over, Transform parent)
        {
            GameObject g = new GameObject(normal.name.Replace("Normal", ""));
            g.transform.SetParent(parent.transform);
            UnityEngine.UI.Button btn = g.AddComponent<UnityEngine.UI.Button>();
            UnityEngine.UI.Image img = g.AddComponent<UnityEngine.UI.Image>();
            //ButtonSpriteSwap buttonSpriteSwap = g.AddComponent<ButtonSpriteSwap>();
            //buttonSpriteSwap.normalSprite = normal;
            //buttonSpriteSwap.clickSprite = click;
            //buttonSpriteSwap.overSprite = over;
            //buttonSpriteSwap.isTouchSound = true;

            btn.image = img;

            img.sprite = normal;
            img.SetNativeSize();
        }

        private void ReturnToSetupPhase(bool restoreInitialSceneSetup)
        {
            if ( restoreInitialSceneSetup && !EditorApplication.isPlaying)
                return;

            errorMessage = string.Empty;
            currentPhase = Phase.Setup;
        }

        private void SaveParticleTextureInFolder(GameObject target)
        {
            if(target == null)
            {
                Debug.LogError("선택된 타겟이 없습니다.");
                return;
            }

            var particles = target.transform.GetComponentsInChildren<ParticleSystem>(true);
            Dictionary<string, List<ParticleSystem>> particleDic = new Dictionary<string, List<ParticleSystem>>();
            List<MaterialTexture> texList = new List<MaterialTexture>();

            if(particles.Length == 0)
            {
                Debug.Log("파티클이 없습니다.");
                return;
            }
            //Debug.Log(particles.Length);

            foreach(var particle in particles)
            {
                var renderer = particle.GetComponent<ParticleSystemRenderer>();
                Material mat = renderer.sharedMaterial;
                Texture tex = renderer.sharedMaterial.mainTexture;
                //Debug.Log(AssetDatabase.GetAssetPath(tex));
                if (tex != null && mat != null)
                {
                    MaterialTexture materialTexture = new MaterialTexture();
                    materialTexture.tex = tex;
                    materialTexture.mat = mat;
                    materialTexture.texid = tex.name;
                    materialTexture.matid = mat.name;

                    if (particleDic.ContainsKey(tex.name))
                    {
                        particleDic[tex.name].Add(particle);
                    }
                    else
                    {
                        List<ParticleSystem> particleList = new List<ParticleSystem>();
                        particleList.Add(particle);
                        particleDic.Add(tex.name, particleList);
                    }

                    bool isContain = false;

                    for (int i = 0; i < texList.Count; i++)
                    {
                        if (texList[i].texid == materialTexture.texid || texList[i].matid == materialTexture.matid)
                        {
                            isContain = true;
                            break;
                        }
                    }

                    if (!isContain)
                    {
                        texList.Add(materialTexture);
                    }
                }
            }

            string openPath = Application.dataPath + AssetDatabase.GetAssetPath(texList[0].mat).Replace("Assets/", "");//Application.dataPath;
            
            string[] split = openPath.Split('/');
            string reStr = "";

            for(int i = 0; i < split.Length - 1; i++)
            {
                reStr = string.Concat(reStr, split[i], "/");
            }

            Debug.Log(reStr);

            string savePath = EditorUtility.SaveFolderPanel("저장 경로를 선택해주세요.", reStr, "");

            if(savePath.Length < 1)
            {
                GC.Collect();
                return;
            }

            foreach (var tex in texList)
            {
                string[] words = savePath.Split('/');
                string saveFile = savePath + "/" + tex.texid + ".png";
                string assetPath = "";

                for (int i = words.Length - 1; i >= 0; i--)
                {
                    assetPath = string.Concat(words[i], "/", assetPath);

                    if (words[i].Contains("Assets"))
                    {
                        break;
                    }
                }

                string matPath = assetPath + "Particle/Material/";
                string texPath = assetPath + "Particle/Textures/";

                DirectoryInfo texDI = new DirectoryInfo(texPath);
                DirectoryInfo matDI = new DirectoryInfo(matPath);

                if (!tex.texid.Contains("Default"))
                {
                    if (!texDI.Exists)
                    {
                        texDI.Create();
                    }

                    AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(tex.tex), texPath + tex.texid + ".png");
                }

                if (!matDI.Exists)
                {
                    matDI.Create();
                }

                AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(tex.mat), matPath + tex.matid + ".mat");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                for (int i = 0; i < particleDic[tex.texid].Count; i++)
                {
                    Material mat = (Material)AssetDatabase.LoadAssetAtPath(matPath + tex.matid + ".mat", typeof(Material));

                    Debug.Log(assetPath + tex.matid + ".mat" + " : " + (mat != null));

                    if (!tex.texid.Contains("Default"))
                    {
                        mat.mainTexture = (Texture)AssetDatabase.LoadAssetAtPath(texPath + tex.texid + ".png", typeof(Texture));
                    }
                    else
                    {
                        mat.mainTexture = tex.tex;
                    }
                        
                    particleDic[tex.texid][i].GetComponent<ParticleSystemRenderer>().sharedMaterial = mat;
                }
            }

            Debug.Log("Complete Save!");
        }

        public void CreateSlotObject()
        {
            //CreateScriptableObject.CreateDefaultGameroot("default");
        }
    }

    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }

    public class MaterialTexture
    {
        public string texid;
        public string matid;
        public Texture tex;
        public Material mat;
    }

    public class ButtonUtility
    {
        public string assetName;
        public Sprite Normal;
        public Sprite Click;
        public Sprite Over;
    }
}

