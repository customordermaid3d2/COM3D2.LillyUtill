using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COM3D2.LillyUtill
{
    public class PresetUtill
    {

        public static string[] presetlist;

        public static void init()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Preset");
            if (Directory.Exists(path))
            {
                presetlist = Directory.GetFiles(path, "*.preset", SearchOption.AllDirectories);
            }
            else
            {
                presetlist = new string[]{};
            }
        }

        public static void SetMaidRandPreset(Maid m_maid)
        {
            SetMaidPreset(m_maid, presetlist[UnityEngine.Random.Range(0, presetlist.Length)]);
        }
        
        public static void SetMaidRandPreset2(Maid m_maid, CharacterMgr.PresetType presetType = CharacterMgr.PresetType.All)
        {
            SetMaidPreset2(m_maid, presetlist[UnityEngine.Random.Range(0, presetlist.Length)], presetType);
        }


        public static void SetMaidPreset(Maid m_maid, string file)
        {
            if (m_maid == null)
            {
                LillyUtill.Log.LogWarning("SetMaidPreset maid null");
                return;
            }
            if (m_maid.IsBusy)
            {
                LillyUtill.Log.LogWarning("RandPreset Maid Is Busy");
                return;
            }

            //if (configEntryUtill["SetMaidPreset", false])
            //    MyLog.LogDebug("SetMaidPreset select :" + file);

            CharacterMgr.Preset preset = GameMain.Instance.CharacterMgr.PresetLoad(file);

            //Main.CustomPresetDirectory = Path.GetDirectoryName(file);
            //UnityEngine.Debug.Log("RandPreset preset path "+ GameMain.Instance.CharacterMgr.PresetDirectory);
            //preset.strFileName = file;
            if (preset == null)
            {
                //  if (configEntryUtill["SetMaidPreset", false])
                LillyUtill.Log.LogDebug("SetMaidPreset preset null ");
                return;
            }            
            GameMain.Instance.CharacterMgr.PresetSet(m_maid, preset);
            if (Product.isPublic)
            {
                SceneEdit.AllProcPropSeqStart(m_maid);
            }
        }

        /// <summary>
        /// PresetType change
        /// </summary>
        /// <param name="m_maid"></param>
        /// <param name="file"></param>
        /// <param name="presetType"></param>
        public static void SetMaidPreset2(Maid m_maid, string file, CharacterMgr.PresetType presetType= CharacterMgr.PresetType.All)
        {
            if (m_maid == null)
            {
                LillyUtill.Log.LogWarning("SetMaidPreset maid null");
                return;
            }
            if (m_maid.IsBusy)
            {
                LillyUtill.Log.LogWarning("RandPreset Maid Is Busy");
                return;
            }

            //if (configEntryUtill["SetMaidPreset", false])
            //    MyLog.LogDebug("SetMaidPreset select :" + file);

            CharacterMgr.Preset preset = GameMain.Instance.CharacterMgr.PresetLoad(file);

            //Main.CustomPresetDirectory = Path.GetDirectoryName(file);
            //UnityEngine.Debug.Log("RandPreset preset path "+ GameMain.Instance.CharacterMgr.PresetDirectory);
            //preset.strFileName = file;
            if (preset == null)
            {
                //  if (configEntryUtill["SetMaidPreset", false])
                LillyUtill.Log.LogDebug("SetMaidPreset preset null ");
                return;
            }
            preset.ePreType = presetType;
            GameMain.Instance.CharacterMgr.PresetSet(m_maid, preset);
            if (Product.isPublic)
            {
                SceneEdit.AllProcPropSeqStart(m_maid);
            }
        }
    }
}
