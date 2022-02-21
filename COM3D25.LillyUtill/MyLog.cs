using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COM3D25.LillyUtill
{
    public class MyLog
    {
        public ManualLogSource log;// = BepInEx.Logging.Logger.CreateLogSource(MyAttribute.PLAGIN_NAME);

        public ConfigEntry<bool> IsLogingLogMessage=null;
        public ConfigEntry<bool> IsLogingLogDebug = null;
        public ConfigEntry<bool> IsLogingLogInfo = null;
        public ConfigEntry<bool> IsLogingLogWarning = null;


        public MyLog(ManualLogSource log)
        {
            init(log);
        }
        
        public MyLog(ManualLogSource log, ConfigFile config)
        {
            init(log, config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public MyLog(string name)
        {
            init( name);
        }
        
        public MyLog(string name, ConfigFile config)
        {
            init( name, config);
        }

        public ManualLogSource GetLog()
        {
            return log;
        }

        public MyLog init(ManualLogSource log)
        {
            this.log =log;
            ConfigEntry(new ConfigFile(Path.Combine(Paths.ConfigPath, "COM3D2." + log.SourceName) + ".MyLog.cfg", true));
            return this;
        }
        
        public MyLog init(ManualLogSource log, ConfigFile config)
        {
            this.log =log;
            ConfigEntry(config); 
            return this;
        }

        public MyLog init(string name)
        {
            log = BepInEx.Logging.Logger.CreateLogSource(name);
            // Path.Combine(Paths.ConfigPath, "COM3D2.HighHeel");
            ConfigEntry(new ConfigFile(Path.Combine(Paths.ConfigPath, "COM3D2."+ name) + ".MyLog.cfg", true));            
            return this;
        }
        
        public MyLog init(string name, ConfigFile config)
        {
            log = BepInEx.Logging.Logger.CreateLogSource(name);
            ConfigEntry(config);
            return this;
        }

        public void ConfigEntry(ConfigFile config)
        {
            IsLogingLogDebug = config.Bind("MyLog", "IsLogingLogDebug", false);
            IsLogingLogInfo = config.Bind("MyLog", "IsLogingLogInfo", false);
            IsLogingLogMessage = config.Bind("MyLog", "IsLogingLogMessage", true);
            IsLogingLogWarning = config.Bind("MyLog", "IsLogingLogWarning", true);
        }

        public void LogLine()
        {
            Console.ForegroundColor = ConsoleColor.White;
            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.BackgroundColor = color;
                Console.Write("=== {0} ===", color);
            }
            Console.WriteLine();
            Console.ResetColor();
        }


        public void LogOut(object[] args, Action<string> action)
        {
            action(MyUtill.Join(" , ", args));
        }

        public void ConsoleOut(object[] args, ConsoleColor consoleColor)
        {

            Console.BackgroundColor = consoleColor;
            Console.WriteLine(MyUtill.Join(" , ", args));
            Console.BackgroundColor = ConsoleColor.Black;
            //Console.ResetColor();            
        }


        public void ConsoleOut(object[] args)
        {

            Console.WriteLine(MyUtill.Join(" , ", args));
            //Console.ResetColor();
        }

        public void LogDebug(params object[] args)
        {
            if (IsLogingLogDebug == null || IsLogingLogDebug.Value)
                LogOut(args, log.LogDebug);
            // ConsoleOut(args);
        }

        public void LogInfo(params object[] args)
        {
            if (IsLogingLogInfo == null || IsLogingLogInfo.Value)
                LogOut(args, log.LogInfo);
            //ConsoleOut(args, ConsoleColor.DarkGray);
        }

        public void LogMessage(params object[] args)
        {
            if (IsLogingLogMessage == null || IsLogingLogMessage.Value)
            {
                LogOut(args, log.LogMessage);
            }
            // ConsoleOut(args, ConsoleColor.DarkBlue);
        }

        public void LogDarkMagenta(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkMagenta);
        }
        public void LogDarkYellow(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkYellow);
        }

        public void LogBlue(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.Blue);
        }

        public void LogDarkRed(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkRed);
        }

        public void LogDarkBlue(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkBlue);
        }

        public void Log(ConsoleColor consoleColor, params object[] args)
        {
            ConsoleOut(args, consoleColor);
        }

        public void Log(params object[] args)
        {
            ConsoleOut(args);
        }

        public void LogWarning(params object[] args)
        {
            if (IsLogingLogWarning == null || IsLogingLogWarning.Value)
            {                
                LogOut(args, log.LogWarning);
            }
            //ConsoleOut(args, ConsoleColor.DarkYellow);
        }

        public void LogFatal(params object[] args)
        {
            LogOut(args, log.LogFatal);
            //ConsoleOut(args, ConsoleColor.Red);
        }

        public void LogError(params object[] args)
        {
            LogOut(args, log.LogError);
            //ConsoleOut(args, ConsoleColor.DarkRed);
        }
    }
}
