using MaidStatus;
using scoutmode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.LillyUtill
{

    public class PersonalUtill
    {
        public static List<Personal.Data> personalDataAll;
        public static List<Personal.Data> personalDataEnable;

        public static List<Personal.Data> GetPersonalData(bool onlyEnabled = true)
        {
            CreateData();
            if (onlyEnabled)
            {
                return personalDataEnable;
            }
            return personalDataAll;
        }

        public static Personal.Data GetPersonalData(int index, bool onlyEnabled = true)
        {
            CreateData();
            if (onlyEnabled)
            {
                return personalDataEnable[index];
            }
            return personalDataAll[index];
        }

        private static void CreateData()
        {
            if (personalDataAll == null)
            {
                LillyUtill.myLog.LogMessage("CreateData.personalDataAll");
                personalDataAll = Personal.GetAllDatas(false);               
            }
            if (personalDataEnable == null)
            {
                LillyUtill.myLog.LogMessage("CreateData.personalDataEnable");
                personalDataEnable = Personal.GetAllDatas(true);

                //bool flag = true;
                //flag = (GameMain.Instance.CharacterMgr.status.GetFlag("オープニング終了") == 1);
                List<Personal.Data> list = new List<Personal.Data>();
                foreach (var data in personalDataEnable)
                {
                    LillyUtill.myLog.LogFatal(data.uniqueName);
                    if (data.oldPersonal)
                    {
                        string a = data.uniqueName.ToLower();
                        if (a == "pure" || a == "cool" || a == "pride")
                        {
                            if (!string.IsNullOrEmpty(GameMain.Instance.CMSystem.CM3D2Path) && PluginData.IsEnabled("Legacy"))
                            //    if (GameMain.Instance.CharacterMgr.status.isAvailableTransfer)
                            {
                                //list.Add(data);
                            }
                            else
                            {
                                list.Add(data);
                            }
                        }
                        //else if (flag)
                        else
                        {
                            if (data.single)
                            {
                                //list.Add(data);
                            }
                            else if (!string.IsNullOrEmpty(GameMain.Instance.CMSystem.CM3D2Path) && data.compatible)
                            {
                                //list.Add(data);
                            }
                            else
                            {
                                list.Add(data);
                            }
                        }
                    }
                    else
                    {
                        //list.Add(data);
                    }
                }
                personalDataEnable= personalDataEnable.Except(list).ToList();
            }

        }

        /*

            bool flag = true;
            flag = (GameMain.Instance.CharacterMgr.status.GetFlag("オープニング終了") == 1);
            //List<Personal.Data> allDatas = Personal.GetAllDatas(true);
            List<Personal.Data> list = new List<Personal.Data>();
            foreach (Personal.Data data in personalDataEnable)
            {
                if (SceneEdit.Instance.modeType != SceneEdit.ModeType.ScoutChara || LockData.personalEnabledIdList.Contains(data.uniqueName))
                {
                    string a = data.uniqueName.ToLower();
                    if (data.oldPersonal)
                    {
                        if (a == "pure" || a == "cool" || a == "pride")
                        {
                            if (GameMain.Instance.CharacterMgr.status.isAvailableTransfer)
                            {
                                list.Add(data);
                            }
                        }
                        else if (flag)
                        {
                            if (data.single)
                            {
                                list.Add(data);
                            }
                            else if (!string.IsNullOrEmpty(GameMain.Instance.CMSystem.CM3D2Path) && data.compatible)
                            {
                                list.Add(data);
                            }
                        }
                    }
                    else
                    {
                        list.Add(data);
                    }
                }
            }
         */

        public static int SetPersonalRandom(Maid maid)
        {
            if (maid == null)
            {
                LillyUtill.myLog.LogFatal("maid null");
            }






            int a = UnityEngine.Random.Range(0, GetPersonalData().Count);
            Personal.Data data = GetPersonalData(a);
            maid.status.SetPersonal(data);
            maid.status.firstName = data.uniqueName;

            return a;
        }

        public static void SetPersonalCare(Maid maid)
        {

            maid.status.SetPersonal(maid.status.personal);

        }

        public static Personal.Data SetPersonal(Maid maid, int index)
        {
            Personal.Data data = GetPersonalData(index);
            maid.status.SetPersonal(data);
            maid.status.firstName = data.uniqueName;

            return data;
        }

    }
}

