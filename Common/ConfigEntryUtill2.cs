using BepInEx.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace COM3D2.LillyUtill
{
    public class ConfigEntryUtill2<T>
    {
        ConfigFile config;
        // section, key
        Dictionary<string, Dictionary<string, ConfigEntry<T>>> list = new Dictionary<string, Dictionary<string, ConfigEntry<T>>>();
        // key, section
        Dictionary<string, Dictionary<string, ConfigEntry<T>>> listKey = new Dictionary<string, Dictionary<string, ConfigEntry<T>>>();
        T defult;

        public ConfigEntryUtill2(ConfigFile config, T defult)
        {
            this.config = config;
            this.defult = defult;
        }

        public static ConfigEntryUtill2<T> Create(ConfigFile config, T defult)
        {
            return new ConfigEntryUtill2<T>(config, defult);
        }

        public Dictionary<string, ConfigEntry<T>> this[string section]
        {
            get
            {
                if (!list.ContainsKey(section))
                {
                    list[section] = new Dictionary<string, ConfigEntry<T>>();
                }
                return list[section];
            }
        }
        
        public T this[string section, string key]
        {
            set
            {
                Setup(section, key);
                this[section][key].Value = value;
            }
            get
            {
                Setup(section, key);
                return this[section][key].Value;
            }
        }

        public void Setup(string section, string key)
        {
            Setup(section, key, defult);
        }

        public void Setup(string section, string key,T defult)
        {
            if (!this[section].ContainsKey(key))
            {
                if (!listKey.ContainsKey(key))
                {
                    listKey[key] = new Dictionary<string, ConfigEntry<T>>();
                }
                listKey[key][section] = this[section][key] = config.Bind(section, key, defult);
            }
        }

        public Dictionary<string, Dictionary<string, ConfigEntry<T>>> GetList()
        {
            return list;
        }

        public Dictionary<string, ConfigEntry<T>> GetList(string key)
        {
            return listKey[key];
        }
    }
}