using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.LillyUtill
{
    public class MyLog
    {
        public ManualLogSource log;// = BepInEx.Logging.Logger.CreateLogSource(MyAttribute.PLAGIN_NAME);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public MyLog(ManualLogSource log)
        {
            init(log);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public MyLog(string name)
        {
            init( name);
        }

        public ManualLogSource GetLog()
        {
            return log;
        }

        public MyLog init(ManualLogSource log)
        {
            this.log =log;
            return this;
        }
        public MyLog init(string name)
        {
            log = BepInEx.Logging.Logger.CreateLogSource(name);
            return this;
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
            LogOut(args, log.LogDebug);
            // ConsoleOut(args);
        }

        public void LogInfo(params object[] args)
        {
            LogOut(args, log.LogInfo);
            //ConsoleOut(args, ConsoleColor.DarkGray);
        }

        public void LogMessage(params object[] args)
        {
            LogOut(args, log.LogMessage);
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
            LogOut(args, log.LogWarning);
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
