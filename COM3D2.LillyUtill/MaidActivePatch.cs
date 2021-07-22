using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.LillyUtill
{
    public class MaidActivePatch
    {
        public static Maid[] maids = new Maid[18];
        public static string[] maidNames = new string[18];

        /// <summary>
        /// if (!f_bMan)
        /// </summary>
        public static event Action setActive = delegate { };

        /// <summary>
        /// if (!f_bMan)
        /// </summary>
        public static event Action deactivate = delegate { };

        /// <summary>
        /// 메이드가 슬롯에 넣었을때 
        /// 
        /// </summary>
        /// <param name="f_maid">어떤 메이드인지</param>
        /// <param name="f_nActiveSlotNo">활성화된 메이드 슬롯 번호. 다시말하면 메이드를 집어넣을 슬롯</param>
        /// <param name="f_bMan">남잔지 여부</param>
        [HarmonyPatch(typeof(CharacterMgr), "SetActive")]
        [HarmonyPostfix]// CharacterMgr의 SetActive가 실행 후에 아래 메소드 작동
        public static void SetActive(Maid f_maid, int f_nActiveSlotNo, bool f_bMan)
        {
            if (!f_bMan)// 남자가 아닐때
            {
                // maids 의 위치랑 maidNames 의 위치가 같게끔 설정한거
                maids[f_nActiveSlotNo] = f_maid; // 내가 만든 메이드 목록중 해당 번호 슬롯에 메이드를 저장
                maidNames[f_nActiveSlotNo] = f_maid.status.fullNameEnStyle;
                setActive();
            }
            LillyUtill.myLog.LogMessage("CharacterMgr.SetActive", f_nActiveSlotNo, f_bMan, f_maid.status.fullNameEnStyle);
        }

        /// <summary>
        /// 메이드가 슬롯에서 빠졌을때
        /// </summary>
        /// <param name="f_nActiveSlotNo"></param>
        /// <param name="f_bMan"></param>
        [HarmonyPatch(typeof(CharacterMgr), "Deactivate")]
        [HarmonyPrefix] // CharacterMgr의 SetActive가 실행 전에 아래 메소드 작동
        public static void Deactivate(int f_nActiveSlotNo, bool f_bMan)
        {
            if (!f_bMan)
            {
                maids[f_nActiveSlotNo] = null;
                maidNames[f_nActiveSlotNo] = string.Empty;
                deactivate();
            }
            LillyUtill.myLog.LogMessage("CharacterMgr.Deactivate", f_nActiveSlotNo, f_bMan);
        }
    }
}
