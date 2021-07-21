using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.LillyUtill
{
    public class MyGUI : MonoBehaviour
    {
        public MyGUI instance;

        public ConfigFile config;

        public ConfigEntry<BepInEx.Configuration.KeyboardShortcut> ShowCounter;

        public int windowId = new System.Random().Next();

        private Vector2 scrollPosition;

        // 위치 저장용 테스트 json
        public MyWindowRect myWindowRect;

        public bool IsOpen
        {
            get => myWindowRect.IsOpen;
            set => myWindowRect.IsOpen = value;
        }

        // GUI ON OFF 설정파일로 저장
        private ConfigEntry<bool> IsGUIOn;

        public bool isGUIOn
        {
            get => IsGUIOn.Value;
            set => IsGUIOn.Value = value;
        }

        public System.Windows.Forms.OpenFileDialog openDialog;
        public System.Windows.Forms.SaveFileDialog saveDialog;
        //VistaOpenFileDialog openDialog = new VistaOpenFileDialog();
        //VistaSaveFileDialog saveDialog = new VistaSaveFileDialog();


        private int seleted;
        private int all;

        public static List<MyGUI> guiList = new List<MyGUI>();

        public string windowName;

        public static T Install<T>(GameObject parent, ConfigFile config, string windowName) where T : MyGUI
        {            
            var instance = parent.GetComponent<T>();
            if (instance == null)
            {
                instance = parent.AddComponent<T>();
                instance.config = config;
                MyLog.LogMessage("MyGUI.Install");
                guiList.Add(instance);
                instance.windowName = windowName;
            }
            return instance;
        }

        /// <summary>
        /// 아까 부모 PresetExpresetXmlLoader 에서 봤던 로직이랑 같음
        /// </summary>
        public void Awake()
        {
            MyLog.LogMessage("PresetExpresetXmlLoaderGUI.OnEnable");

            myWindowRect = new MyWindowRect(config, windowName);
            IsGUIOn = config.Bind("GUI", "isGUIOn", false); // 이건 베핀 설정값 지정용
            // 이건 단축키
            ShowCounter = config.Bind("GUI", "isGUIOnKey", new BepInEx.Configuration.KeyboardShortcut(KeyCode.Alpha3, KeyCode.LeftControl));

            // 이건 기어메뉴 아이콘
            SystemShortcutAPI.AddButton(
                MyAttribute.PLAGIN_FULL_NAME
                , new Action(delegate ()
                { // 기어메뉴 아이콘 클릭시 작동할 기능
                    PresetExpresetXmlLoaderGUI.isGUIOn = !PresetExpresetXmlLoaderGUI.isGUIOn;
                })
                , MyAttribute.PLAGIN_NAME + " : " + ShowCounter.Value.ToString() // 표시될 툴팁 내용
                                                                                 // 표시될 아이콘
                , MyUtill.ExtractResource(COM3D2.PresetExpresetXmlLoader.Plugin.Properties.Resources.icon));
            // 아이콘은 이렇게 추가함

            // 파일 열기창 설정 부분. 이런건 구글링 하기
            openDialog = new System.Windows.Forms.OpenFileDialog()
            {
                // 기본 확장자
                DefaultExt = "xml",
                // 기본 디렉토리
                InitialDirectory = Path.Combine(GameMain.Instance.SerializeStorageManager.StoreDirectoryPath, "preset"),
                // 선택 가능 확장자
                Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*"
            };
            saveDialog = new System.Windows.Forms.SaveFileDialog()
            {
                DefaultExt = "xml",
                InitialDirectory = Path.Combine(GameMain.Instance.SerializeStorageManager.StoreDirectoryPath, "preset"),
                Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*"
            };

        }
        // 이렇게 해서 플러그인 실행 직후는 작동 완료

        public void OnEnable()
        {
            MyLog.LogMessage("PresetExpresetXmlLoaderGUI.OnEnable");

            PresetExpresetXmlLoaderGUI.myWindowRect.load();// 이건 창 위치 설정하는건데 소스 열어서  다로 공부해볼것
            SceneManager.sceneLoaded += this.OnSceneLoaded;
        }

        public void Start()
        {
            MyLog.LogMessage("PresetExpresetXmlLoaderGUI.Start");
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PresetExpresetXmlLoaderGUI.myWindowRect.save();// 장면 이동시 GUI 창 위치 저장
        }

        private void Update()
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
                MyLog.LogMessage("IsUp", ShowCounter.Value.MainKey);
            }
        }

        // 매 화면 갱신할때마다(update 말하는게 아님)
        public void OnGUI()
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
        public void WindowFunction(int id)
        {
            GUI.enabled = true; // 기능 클릭 가능

            GUILayout.BeginHorizontal();// 가로 정렬
            // 라벨 추가
            GUILayout.Label(MyAttribute.PLAGIN_NAME + " " + ShowCounter.Value.ToString(), GUILayout.Height(20));
            // 안쓰는 공간이 생기더라도 다른 기능으로 꽉 채우지 않고 빈공간 만들기
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) { IsOpen = !IsOpen; }
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20))) { isGUIOn = false; }
            GUILayout.EndHorizontal();// 가로 정렬 끝

            if (!IsOpen)
            {

            }
            else
            {
                // 스크롤 영역
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                // 메이드가 있을때만 여기 아래 기능들 클릭 가능
                GUI.enabled = PresetExpresetXmlLoaderPatch.maids[seleted] != null;
                if (GUI.enabled)
                {
                    GUILayout.Label("");
                }
                else
                {
                    GUILayout.Label("maid null");
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("load"))
                {
                    if (!isShowDialogLoadRun)
                    {
                        //Task.Factory.StartNew(ShowDialogLoadRun);
                        ShowDialogLoadRun();
                    }
                }
                if (GUILayout.Button("save"))
                {
                    if (!isShowDialogSaveRun)
                    {
                        //Task.Factory.StartNew(ShowDialogSaveRun);
                        ShowDialogSaveRun();
                    }
                }

                GUILayout.EndHorizontal();

                GUI.enabled = true;
                GUILayout.Label("option");

                all = GUILayout.SelectionGrid(all, type, 2);
                if (all == 1)
                {
                    GUI.enabled = false;
                }

                GUILayout.Label("maid select");
                // 여기는 출력된 메이드들 이름만 가져옴
                // seleted 가 이름 위치 번호만 가져온건데
                seleted = GUILayout.SelectionGrid(seleted, PresetExpresetXmlLoaderPatch.maidNames, 1);


                GUILayout.EndScrollView();
            }
            GUI.enabled = true;
            GUI.DragWindow(); // 창 드레그 가능하게 해줌. 마지막에만 넣어야함
        }

        static bool isShowDialogSaveRun = false;
        static bool isShowDialogLoadRun = false;

        private void ShowDialogSaveRun()
        {
            isShowDialogSaveRun = true;
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (all == 0)
                {
                    PresetExpresetXmlLoaderUtill.Save(seleted, saveDialog.FileName);
                }
                else
                {
                    int s = saveDialog.FileName.LastIndexOf(".xml");
                    for (int i = 0; i < 18; i++)
                    {
                        PresetExpresetXmlLoaderUtill.Save(i, saveDialog.FileName.Insert(s, "_" + PresetExpresetXmlLoaderPatch.maidNames[i]));
                    }
                }
            }
            isShowDialogSaveRun = false;
        }

        /// <summary>
        /// 원래 함수를 별도로 만든게 아닌데 오류땜에 뺏다가 현상 유지됨..
        /// </summary>
        private void ShowDialogLoadRun()
        {
            isShowDialogLoadRun = true;
            if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)// 오픈했을때
            {
                if (all == 0)
                {
                    PresetExpresetXmlLoaderUtill.Load(seleted, openDialog.FileName);
                    // 파일 읽고 메이드에게 처리해주는 기능 
                }
                else
                {
                    for (int i = 0; i < 18; i++)
                        PresetExpresetXmlLoaderUtill.Load(i, openDialog.FileName);
                }
            }
            isShowDialogLoadRun = false;
        }

        /// <summary>
        /// 게임 X 버튼 눌렀을때 반응
        /// </summary>
        public void OnApplicationQuit()
        {
            PresetExpresetXmlLoaderGUI.myWindowRect.save();
            MyLog.LogMessage("OnApplicationQuit");
        }

        /// <summary>
        /// 게임 종료시에도 호출됨
        /// </summary>
        public void OnDisable()
        {

            PresetExpresetXmlLoaderGUI.myWindowRect.save();
            SceneManager.sceneLoaded -= this.OnSceneLoaded;
        }


    }
}
