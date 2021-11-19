﻿using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.LillyUtill
{
    class MyAttribute
    {
        public const string PLAGIN_NAME = "LillyUtill";
        public const string PLAGIN_VERSION = "21.8.15.21";
        public const string PLAGIN_FULL_NAME = "COM3D2.LillyUtill.Plugin";
    }

    [BepInPlugin(MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, MyAttribute.PLAGIN_VERSION)]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    //[BepInPlugin("COM3D2.Sample.Plugin", "COM3D2.Sample.Plugin", "21.6.6")]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    [BepInProcess("COM3D2x64.exe")]
    class LillyUtill : BaseUnityPlugin
    {
        public static MyLog myLog;
        public static Harmony maidActivePatch;

        public LillyUtill()
        {
            myLog = new MyLog(MyAttribute.PLAGIN_NAME);
        }

        public void Awake()
        {
            myLog.LogMessage("Awake");
            maidActivePatch=Harmony.CreateAndPatchAll(typeof(MaidActivePatch));
            PresetUtill.init();
            MaidActivePatch.init();
            //MaidActivePatch.maidCntChg(3);

        }

        public void OnDisable()
        {
            myLog.LogMessage("OnDisable");
            MyWindowRect.ActionSave();
        }


    }
}
