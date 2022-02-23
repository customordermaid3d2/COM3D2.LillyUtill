using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.LillyUtill
{

    [Obsolete("미구현")]
    public class MaidActivePatch2
    {

        internal static void Awake()
        {

        }


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
                //LillyUtill.Log.LogInfo("CharacterMgr.SetActive", f_maid.status.fullNameEnStyle);

                try
                {
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.setActive {e.ToString()}");
                }
                
            }
            //LillyUtill.Log.LogInfo("CharacterMgr.SetActive", f_nActiveSlotNo, f_bMan, f_maid.status.fullNameEnStyle);
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
                //LillyUtill.Log.LogInfo("CharacterMgr.Deactivate", f_nActiveSlotNo);

                try
                {
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.SetMaid {e.ToString()}");
                }
            }
            //LillyUtill.Log.LogInfo("CharacterMgr.Deactivate", f_nActiveSlotNo, f_bMan);
        }

    }
}
