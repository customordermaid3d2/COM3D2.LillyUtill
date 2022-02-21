using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D25.LillyUtill
{
    public class MaidActivePatch
    {
        //[Obsolete("use public static Maid GetMaid(int select) or Maids2")]
        public static Maid[] maids = new Maid[3];
        //[Obsolete("use public static string GetMaidName(int select)")]
        /// <summary>
        /// seleted = MaidActivePatch.SelectionGrid (seleted,3,265,false) ;
        /// </summary>
        public static string[] maidNames = new string[3];

        private static Dictionary<int, Maid> maids2 = new Dictionary<int, Maid>();
        private static Dictionary<int, string> maidNames2 = new Dictionary<int, string>();

        public static Dictionary<int, Maid> Maids2
        {
            get => maids2;
            //set => maidNames = value;
        }
        
        public static Dictionary<int, string> MaidNames2
        {
            get => maidNames2;
            //set => maidNames = value;
        }

        private static int max = 2;
        private static int maxb = 2;
        

        public static int Max { get => max; }


        const int c_max = 6;//41-9=32 25*.25

        /// <summary>
        /// HarmonyPatch(typeof(CharacterMgr), "SetActive") HarmonyPostfix
        /// </summary>
        public static event Action setActive = delegate { };
        public static event Action<Maid> setActiveMaid = delegate { };
        /// <summary>
        /// HarmonyPatch(typeof(CharacterMgr), "SetActive") HarmonyPostfix
        /// </summary>
        public static event Action<int> setActiveMaid2 = delegate { };
        /// <summary>
        /// HarmonyPatch(typeof(CharacterMgr), "SetActive") HarmonyPostfix
        /// </summary>
        public static event Action<int, Maid> setActiveMaid3 = delegate { };


        private static int selected = 0;

        /// <summary>
        /// Maid SelectMaid(int select)
        /// MaidActivePatch.seleced
        /// Action<int> selecedMaid
        /// </summary>
        public static int Selected { get => selected; }

        /// <summary>
        /// Maid SelectMaid(int select)
        /// MaidActivePatch.seleced
        /// Action<int> selecedMaid
        /// </summary>
        public static event Action<int> selectedMaid = delegate { };

        /// <summary>
        /// if (!f_bMan)
        /// </summary>
       
        public static event Action deactivate = delegate { };
        public static event Action<int> deactivateMaid = delegate { };
        public static event Action<Maid> deactivateMaid2 = delegate { };
        public static event Action<int, Maid> deactivateMaid3 = delegate { };

        /// <summary>
        /// Awake, SetActive, Deactivate run
        /// </summary>
        public static event Action<int> maidCntChg = delegate { };

        internal static void Awake()
        {
            MaidActivePatch.maidCntChg(3);
        }

        public static Maid[] GetMaidAll()
        {
            return maids2.Values.ToArray();
        }

        /// <summary>
        /// Maid SelectMaid(int select)
        /// MaidActivePatch.seleced
        /// selecedMaid
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public static Maid SelectMaid(int select)
        {
            if (maids2.ContainsKey(select))
            {
                MaidActivePatch.selected = select;
                selectedMaid(select);
                return maids2[select];
            }
            MaidActivePatch.selected = 0;
            selectedMaid(0);
            return null;
        }        

        public static Maid GetMaid(int select)
        {
            if (maids2.ContainsKey(select))
            {
                return maids2[select];
            }
            return null;
        }

        public static string GetMaidName(int select)
        {
            if (maidNames2.ContainsKey(select))
            {
                return maidNames2[select];
            }
            return string.Empty;
        }

        private static void SetMaid(int select, Maid maid)
        {
            if (maid == null)
            {
                if (maids2.ContainsKey(select))
                {
                    maids2.Remove(select);
                    maidNames2.Remove(select);
                }
            }
            else if (maids2.ContainsKey(select))
            {
                maids2[select] = maid;
                maidNames2[select] = maid.status.fullNameEnStyle;
            }
            else
            {
                maids2.Add(select, maid);
                maidNames2.Add(select, maid.status.fullNameEnStyle);
            }

            int c = 0;
            if (maids2.Count > 0)
            {
                c = maids2.Keys.Max() + 1;//1
            }

            int i1 = c / 3;//0
            int i2 = (i1 + 1) * 3 - c;//1

            // max =  (c / 3 + 1) * 3 ;

            if (i2 == 3)
            {
                max = c;
            }
            else
            {
                max = c + i2;//3
            }

            //LillyUtill.Log.LogInfo("MaidActivePatch.SetMaid1", c, i1, i2, max, maxb);

            //if (max < c_max)
            //{
            //    max = c_max;
            //}

            if (maxb != max)
            {
                Array.Resize(ref maids, max);
                Array.Resize(ref maidNames, max);
                //LillyUtill.Log.LogInfo("MaidActivePatch.SetMaid2", maids.Length, maidNames.Length);
                //maids = new Maid[max];
                //maidNames = new string[max];
                if (maxb < max)
                    for (int i = maxb ; i < max; i++)
                    {
                        //if (maids2.ContainsKey(i))
                        //{
                        //    maids[i] = maids2[i];
                        //    maidNames[i] = maidNames2[i];
                        //}
                        //else
                        {
                            maids[i] = null;
                            maidNames[i] = string.Empty;
                        }
                    }
                maidCntChg(max);
            }
            //else
            {
                //LillyUtill.Log.LogInfo("MaidActivePatch.SetMaid3", select , max);
                if (select < max)
                {
                    maids[select] = maid;
                    if (maid == null)
                    {
                        maidNames[select] = string.Empty;
                    }
                    else
                    {
                        maidNames[select] = maid.status.fullNameEnStyle;
                    }
                }
            }
            maxb = max;
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
                    SetMaid(f_nActiveSlotNo, f_maid);
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogWarning($"CharacterMgr.SetMaid {e.ToString()}" );
                }
                // maids 의 위치랑 maidNames 의 위치가 같게끔 설정한거
                //if (f_nActiveSlotNo<18)
                //{
                //    Maids[f_nActiveSlotNo] = f_maid; // 내가 만든 메이드 목록중 해당 번호 슬롯에 메이드를 저장
                //    MaidNames[f_nActiveSlotNo] = f_maid.status.fullNameEnStyle;
                //}
                try
                {
                    setActive();
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.setActive {e.ToString()}" );
                }
                try
                {
                    setActiveMaid(f_maid);
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.setActiveMaid {e.ToString()}");
                }
                try
                {
                    setActiveMaid2(f_nActiveSlotNo);
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.setActiveMaid2 {e.ToString()}");
                }
                try
                {
                    setActiveMaid3(f_nActiveSlotNo, f_maid);
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.setActiveMaid3 {e.ToString()}");
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
                    deactivate();
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.deactivate {e.ToString()}");
                }
                try
                {
                    if (maids2.ContainsKey(f_nActiveSlotNo))
                        deactivateMaid(f_nActiveSlotNo);
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.deactivateMaid {e.ToString()}");
                }
                try
                {
                    if (maids2.ContainsKey(f_nActiveSlotNo))
                    {
                        deactivateMaid2(maids2[f_nActiveSlotNo]);
                    }
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.deactivateMaid2 {e.ToString()}");
                }
                try
                {
                    if (maids2.ContainsKey(f_nActiveSlotNo))
                        deactivateMaid3(f_nActiveSlotNo, maids2[f_nActiveSlotNo]);
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.deactivateMaid3 {e.ToString()}");
                }
                // 이건 무조건 나중에
                try
                {
                    SetMaid(f_nActiveSlotNo, null);
                }
                catch (Exception e)
                {
                    LillyUtill.Log.LogFatal($"CharacterMgr.SetMaid {e.ToString()}");
                }
            }
            //LillyUtill.Log.LogInfo("CharacterMgr.Deactivate", f_nActiveSlotNo, f_bMan);
        }

        [Obsolete("use selectionGrid2")]
        public static event Action selectionGrid = delegate { };
        public static event Action<int> selectionGrid2 = delegate { };

        const float cWidth = 265;

        /// <summary>
        /// GUI.changed = false; after selectionGrid action
        /// </summary>
        /// <param name="seleted"></param>
        /// <param name="cul"></param>
        /// <returns></returns>
        [Obsolete("use SelectionGrid(int seleted, int cul = 3,float Width=265, bool changed = false)")]
        public static int SelectionGrid(int seleted, int cul = 3, bool changed = false)
        {
            return SelectionGrid(seleted, cul, cWidth, changed);
        }

        /// <summary>
        /// event Action selectionGrid run
        /// GUI.changed = false;
        /// </summary>
        /// <param name="seleted"></param>
        /// <param name="cul"></param>
        /// <param name="Width"></param>
        /// <param name="changed"></param> selectionGrid 액션을 호출할지 여부. 호출후 GUI.changed = false;로 됨.
        /// <returns></returns>
        public static int SelectionGrid(int seleted, int cul = 3, float Width = cWidth, bool changed = false)
        {
            GUI.changed = changed;
            GUILayout.Label("maid select");
            seleted = GUILayout.SelectionGrid(seleted, maidNames, cul, GUILayout.Width(Width));
            if (GUI.changed)
            {
                MaidActivePatch.selected = seleted;
                selectionGrid();
                GUI.changed = false;
            }
            return seleted;
        }

        /// <summary>
        /// no event action selectionGrid,selectionGrid2
        /// </summary>ㅇ
        /// <param name="seleted"></param> 메이드 슬롯번호
        /// <param name="cul"></param> 가로 버튼 갯수
        /// <param name="Width"></param> 가로 GUI 크기
        /// <param name="changed"></param> 미사용
        /// <returns></returns>
        public static int SelectionGrid3(int seleted, int cul = 3, float Width = cWidth, bool changed = false )
        {
            MaidActivePatch.selected = seleted;
            return GUILayout.SelectionGrid(seleted, maidNames, cul, GUILayout.Width(Width)); 
        }


        /// <summary>
        /// event Action selectionGrid run
        /// </summary>
        /// <param name="seleted"></param>
        /// <param name="cul"></param>
        /// <param name="Width"></param>
        /// <param name="changed"></param>
        /// <returns></returns>
        public static int SelectionGrid2(int seleted, int cul = 3, float Width = cWidth, bool changed = false)
        {
            GUI.changed = changed;
            GUILayout.Label("maid select");
            seleted = GUILayout.SelectionGrid(seleted, maidNames, cul, GUILayout.Width(Width));
            if (GUI.changed)
            {
                MaidActivePatch.selected = seleted;
                selectionGrid();
                selectionGrid2(seleted);
                GUI.changed = false;
            }
            return seleted;
        }

        [Obsolete("use SelectionGrid(int seleted, int cul = 3,float Width=265, bool changed = false)")]
        public static int SelectionGrid(int seleted, int cul = 3)
        {
            return SelectionGrid(seleted, cul, cWidth, false);
        }

        [Obsolete("use SelectionGrid(int seleted, int cul = 3,float Width=275, bool changed = false)")]
        public static int SelectionGrid(int seleted)
        {
            return SelectionGrid(seleted, 3, cWidth, false);
        }


    }
}
