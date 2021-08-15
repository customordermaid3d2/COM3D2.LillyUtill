using BepInEx.Configuration;
using COM3D2API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace COM3D2.LillyUtill
{
    /// <summary>
    /// 결국 실패
    /// </summary>



    public class MyGUI : MonoBehaviour
    {

        public MyGUI instance;

        public ConfigFile config;

        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter;
        //private KeyboardShortcut keyboardShortcut;

        public int windowId = new System.Random().Next();

        private Vector2 scrollPosition;

        // 위치 저장용 테스트 json
        public MyWindowRect myWindowRect;

        public bool IsOpen
        {
            get => myWindowRect.IsOpen;
            set
            {
                myWindowRect.IsOpen = value;
                if (value)
                {
                    windowName = FullName;
                }
                else
                {
                    windowName = ShortName;
                }
            }
        }

        // GUI ON OFF 설정파일로 저장
        private ConfigEntry<bool> IsGUIOn;

        public bool isGUIOn
        {
            get => IsGUIOn.Value;
            set => IsGUIOn.Value = value;
        }

        public static List<MyGUI> guiList = new List<MyGUI>();

        public string windowName;
        public string FullName;
        public string ShortName;

        // public Bitmap icon;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="config"></param>
        /// <param name="windowName"></param>
        /// <param name="icon"></param>
        /// <param name="keyboardShortcut">new BepInEx.Configuration.KeyboardShortcut(KeyCode.Alpha3, KeyCode.LeftControl)</param>
        /// <returns></returns>
        public static T Install<T>(GameObject parent, ConfigFile config, string FullName, string ShortName, Bitmap icon, KeyboardShortcut keyboardShortcut) where T : MyGUI
        {
            var instance = parent.GetComponent<T>();
            if (instance == null)
            {
                instance = parent.AddComponent<T>();
                //calls Start() on the object and initializes it.
                guiList.Add(instance);
                //MyLog.LogMessage("MyGUI.Install");
                instance.StartAfterSetup(config, FullName, ShortName, keyboardShortcut);

                // 이건 기어메뉴 아이콘
                SystemShortcutAPI.AddButton(
                    FullName + " " + keyboardShortcut.ToString()
                    , new Action(delegate ()
                    { // 기어메뉴 아이콘 클릭시 작동할 기능
                        instance.isGUIOn = !instance.isGUIOn;
                    })
                    , FullName // 표시될 툴팁 내용                               
                , MyUtill.ExtractResource(icon));// 표시될 아이콘
                // 아이콘은 이렇게 추가함

            }
            return instance;
        }

        public static T Install<T>(GameObject parent, ConfigFile config, string configFileName, string FullName, string ShortName, Bitmap icon, KeyboardShortcut keyboardShortcut) where T : MyGUI
        {
            var instance = parent.GetComponent<T>();
            if (instance == null)
            {
                instance = parent.AddComponent<T>();
                //calls Start() on the object and initializes it.
                guiList.Add(instance);
                //MyLog.LogMessage("MyGUI.Install");
                instance.StartAfterSetup(config, configFileName, FullName, ShortName, keyboardShortcut);

                // 이건 기어메뉴 아이콘
                SystemShortcutAPI.AddButton(
                    FullName +" "+ keyboardShortcut.ToString()
                    , new Action(delegate ()
                    { // 기어메뉴 아이콘 클릭시 작동할 기능
                        instance.isGUIOn = !instance.isGUIOn;
                    })
                    , FullName // 표시될 툴팁 내용                               
                , MyUtill.ExtractResource(icon));// 표시될 아이콘
                // 아이콘은 이렇게 추가함

            }
            return instance;
        }

        public void StartAfterSetup(ConfigFile config, string configFileName, string FullName, string ShortName, KeyboardShortcut keyboardShortcut)
        {
            this.config = config;
            this.windowName = FullName;
            this.FullName = FullName;
            this.ShortName = ShortName;
            //this.keyboardShortcut = keyboardShortcut;
            this.IsGUIOn = config.Bind("GUI", "isGUIOn", false); // 이건 베핀 설정값 지정용                                                                         
            this.ShowCounter = config.Bind("GUI", "isGUIOnKey", keyboardShortcut);// 이건 단축키
            this.myWindowRect = new MyWindowRect(config, configFileName, FullName, ShortName);
            IsOpen = IsOpen;
        }

        public void StartAfterSetup(ConfigFile config, string FullName, string ShortName, KeyboardShortcut keyboardShortcut)
        {
            this.config = config;
            this.windowName = FullName;
            this.FullName = FullName;
            this.ShortName = ShortName;
            //this.keyboardShortcut = keyboardShortcut;
            this.IsGUIOn = config.Bind("GUI", "isGUIOn", false); // 이건 베핀 설정값 지정용                                                                         
            this.ShowCounter = config.Bind("GUI", "isGUIOnKey", keyboardShortcut);// 이건 단축키
            this.myWindowRect = new MyWindowRect(config, FullName, FullName, ShortName);
            IsOpen = IsOpen;
        }

        /// <summary>
        /// 아까 부모 PresetExpresetXmlLoader 에서 봤던 로직이랑 같음
        /// </summary>
        public virtual void Awake()
        {
            //MyLog.LogMessage("PresetExpresetXmlLoaderGUI.OnEnable");
        }
        // 이렇게 해서 플러그인 실행 직후는 작동 완료

        public virtual void OnEnable()
        {
            //MyLog.LogMessage("PresetExpresetXmlLoaderGUI.OnEnable");

            myWindowRect?.load();// 이건 창 위치 설정하는건데 소스 열어서  다로 공부해볼것
            SceneManager.sceneLoaded += this.OnSceneLoaded;
        }

        public virtual void Start()
        {
            //MyLog.LogMessage("PresetExpresetXmlLoaderGUI.Start");
            //myWindowRect = new MyWindowRect(config, windowName);
        }

        public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            myWindowRect.save();// 장면 이동시 GUI 창 위치 저장
        }

        public virtual void Update()
        {
            //if (ShowCounter.Value.IsDown())
            //{
            //    MyLog.LogMessage("IsDown", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            //}
            //if (ShowCounter.Value.IsPressed())
            //{
            //    MyLog.LogMessage("IsPressed", ShowCounter.Value.Modifiers, ShowCounter.Value.MainKey);
            //}
            // 단축키 눌렀을때 GUI 키고 끌수 있게 해주는 부분
            if (ShowCounter.Value.IsUp())// 단축키가 일치할때
            {
                isGUIOn = !isGUIOn;// 보이거나 안보이게. 이런 배열이였네 지웠음
                //MyLog.LogMessage("IsUp", ShowCounter.Value.MainKey);
            }
        }

        // 매 화면 갱신할때마다(update 말하는게 아님)
        public virtual void OnGUI()
        {
            if (!isGUIOn)
                return;

            //GUI.skin.window = ;

            //myWindowRect.WindowRect = GUILayout.Window(windowId, myWindowRect.WindowRect, WindowFunction, MyAttribute.PLAGIN_NAME + " " + ShowCounter.Value.ToString(), GUI.skin.box);
            // 별도 창을 띄우고 WindowFunction 를 실행함. 이건 스킨 설정 부분인데 따로 공부할것
            myWindowRect.WindowRect = GUILayout.Window(windowId, myWindowRect.WindowRect, WindowFunction, "", GUI.skin.box);
        }

        string[] type = new string[] { "one", "all" };

        // 창일 따로 뜬 부분에서 작동
        public virtual void WindowFunction(int id)
        {
            GUI.enabled = true; // 기능 클릭 가능

            GUILayout.BeginHorizontal();// 가로 정렬
            // 라벨 추가
            GUILayout.Label(windowName, GUILayout.Height(20));
            // 안쓰는 공간이 생기더라도 다른 기능으로 꽉 채우지 않고 빈공간 만들기
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) { IsOpen = !IsOpen; }
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20))) { isGUIOn = false; }
            GUI.changed = false;

            GUILayout.EndHorizontal();// 가로 정렬 끝

            if (!IsOpen)
            {

            }
            else
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

                WindowFunctionBody(id);

                GUILayout.EndScrollView();
            }
            GUI.enabled = true;
            GUI.DragWindow(); // 창 드레그 가능하게 해줌. 마지막에만 넣어야함
        }

        public virtual void WindowFunctionBody(int id)
        {

        }

        /// <summary>
        /// 게임 X 버튼 눌렀을때 반응
        /// </summary>
        public virtual void OnApplicationQuit()
        {
            myWindowRect?.save();
            //MyLog.LogMessage("OnApplicationQuit");
        }

        /// <summary>
        /// 게임 종료시에도 호출됨
        /// </summary>
        public virtual void OnDisable()
        {
            myWindowRect?.save();
            SceneManager.sceneLoaded -= this.OnSceneLoaded;
        }


    }

#if DEBUG
#endif
}
