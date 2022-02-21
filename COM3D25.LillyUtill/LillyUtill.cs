using BepInEx;
using BepInEx.Configuration;
using COM3D2API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D25.LillyUtill
{
    class MyAttribute
    {
        public const string PLAGIN_NAME = "LillyUtill";
        public const string PLAGIN_VERSION = "22.2.22.05";
        public const string PLAGIN_FULL_NAME = "COM3D25.LillyUtill.Plugin";
    }

    [BepInPlugin(MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, MyAttribute.PLAGIN_VERSION)]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    //[BepInPlugin("COM3D2.Sample.Plugin", "COM3D2.Sample.Plugin", "21.6.6")]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    //[BepInProcess("COM3D2x64.exe")]
    public class LillyUtill : BaseUnityPlugin
    {
        public static MyLog myLog;
        public static Harmony maidActivePatch;
        public static ConfigFile config;

        public MyWindowRect myWindowRect;
        public int windowId = new System.Random().Next();
        //private ConfigEntry<bool> IsGUIOn;
        private Vector2 scrollPosition;

        public bool IsOpen
        {
            get => myWindowRect.IsOpen;
            set
            {
                myWindowRect.IsOpen = value;
            }
        }

        public LillyUtill() : base()
        {
            config = Config;
            ConfigEntryUtill.init(Config);
            ConfigEntryUtill<int>.init(Config);
            PresetUtill.init();
        }

        public void Awake()
        {
            myLog = new MyLog(Logger, Config);
            myLog.LogMessage("Awake https://github.com/customordermaid3d2/COM3D2.LillyUtill");
            maidActivePatch=Harmony.CreateAndPatchAll(typeof(MaidActivePatch));
            //maidActivePatch=Harmony.CreateAndPatchAll(typeof(MaidActivePatch2));
            MyWindowRect.Awake(config);            
            MaidActivePatch.Awake();

            //MaidActivePatch2.Awake();
            //MaidActivePatch.maidCntChg(3);

            MyWindowRect.init();
            myWindowRect = new MyWindowRect(config, MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, "LU", wo: 200, ho: 300);
            
            // 이건 기어메뉴 아이콘
            SystemShortcutAPI.AddButton(
                MyAttribute.PLAGIN_FULL_NAME
                , new Action(delegate ()
                { // 기어메뉴 아이콘 클릭시 작동할 기능
                    myWindowRect.IsGUIOn = !myWindowRect.IsGUIOn;
                })
                , MyAttribute.PLAGIN_NAME // 표시될 툴팁 내용                               
            , MyUtill.ExtractResource(Properties.Resources.icon));// 표시될 아이콘
        }

        public void OnDisable()
        {
            myLog.LogMessage("OnDisable");
            MyWindowRect.ActionSave();
        }

       //public void Start()
       //{                        
       //    //IsGUIOn = config.Bind("GUI", "isGUIOn", false);
       //}

        public void OnGUI()
        {
            if (!myWindowRect.IsGUIOn)
                return;

            //GUI.skin.window = ;

            //myWindowRect.WindowRect = GUILayout.Window(windowId, myWindowRect.WindowRect, WindowFunction, MyAttribute.PLAGIN_NAME + " " + ShowCounter.Value.ToString(), GUI.skin.box);
            // 별도 창을 띄우고 WindowFunction 를 실행함. 이건 스킨 설정 부분인데 따로 공부할것
            myWindowRect.WindowRect = GUILayout.Window(windowId, myWindowRect.WindowRect, WindowFunction, "", GUI.skin.box);
        }

        public virtual void WindowFunction(int id)
        {
            GUI.enabled = true; // 기능 클릭 가능

            GUILayout.BeginHorizontal();// 가로 정렬
            // 라벨 추가
            GUILayout.Label(myWindowRect.windowName, GUILayout.Height(20));
            // 안쓰는 공간이 생기더라도 다른 기능으로 꽉 채우지 않고 빈공간 만들기
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) { IsOpen = !IsOpen; }
            if (GUILayout.Button("x", GUILayout.Width(20), GUILayout.Height(20))) { myWindowRect.IsGUIOn = false; }
            GUI.changed = false;

            GUILayout.EndHorizontal();// 가로 정렬 끝

            if (!IsOpen)
            {

            }
            else
            {
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

                #region 여기에 내용 작성

                if (GUILayout.Button("GUICloseAll", GUILayout.Height(20))) { MyWindowRect.GUICloseAll(); }
                if (GUILayout.Button("GUIMinAll", GUILayout.Height(20))) { MyWindowRect.GUIMinAll(); }
                if (GUILayout.Button("GUISortAll", GUILayout.Height(20))) { MyWindowRect.GUISortAll(); }
                if (GUILayout.Button("GUISortOn", GUILayout.Height(20))) { MyWindowRect.GUISortIsGUIOn(); }
                if (GUILayout.Button("GUISortOpen", GUILayout.Height(20))) { MyWindowRect.GUISortIsOpen(); }

                #endregion

                GUILayout.EndScrollView();
            }

            GUI.enabled = true;
            GUI.DragWindow(); // 창 드레그 가능하게 해줌. 마지막에만 넣어야함
        }
    }
}
