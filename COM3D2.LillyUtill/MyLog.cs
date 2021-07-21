using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.LillyUtill
{
    public class MyLog
    {
        static ManualLogSource log;// = BepInEx.Logging.Logger.CreateLogSource(MyAttribute.PLAGIN_NAME);

        public static void init(string name)
        {
            log = BepInEx.Logging.Logger.CreateLogSource(name);
        }

        public static void LogLine()
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


        public static void LogOut(object[] args, Action<string> action)
        {

            action(MyUtill.Join(" , ", args));
        }

        public static void ConsoleOut(object[] args, ConsoleColor consoleColor)
        {

            Console.BackgroundColor = consoleColor;
            Console.WriteLine(MyUtill.Join(" , ", args));
            Console.BackgroundColor = ConsoleColor.Black;
            //Console.ResetColor();            
        }


        public static void ConsoleOut(object[] args)
        {

            Console.WriteLine(MyUtill.Join(" , ", args));
            //Console.ResetColor();
        }

        public static void LogDebug(params object[] args)
        {
            LogOut(args, log.LogDebug);
            // ConsoleOut(args);
        }

        public static void LogInfo(params object[] args)
        {
            LogOut(args, log.LogInfo);
            //ConsoleOut(args, ConsoleColor.DarkGray);
        }

        public static void LogMessage(params object[] args)
        {
            LogOut(args, log.LogMessage);
            // ConsoleOut(args, ConsoleColor.DarkBlue);
        }

        public static void LogDarkMagenta(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkMagenta);
        }
        public static void LogDarkYellow(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkYellow);
        }

        public static void LogBlue(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.Blue);
        }

        public static void LogDarkRed(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkRed);
        }

        public static void LogDarkBlue(params object[] args)
        {
            ConsoleOut(args, ConsoleColor.DarkBlue);
        }

        public static void Log(ConsoleColor consoleColor, params object[] args)
        {
            ConsoleOut(args, consoleColor);
        }

        public static void Log(params object[] args)
        {
            ConsoleOut(args);
        }

        public static void LogWarning(params object[] args)
        {
            LogOut(args, log.LogWarning);
            //ConsoleOut(args, ConsoleColor.DarkYellow);
        }

        public static void LogFatal(params object[] args)
        {
            LogOut(args, log.LogFatal);
            //ConsoleOut(args, ConsoleColor.Red);
        }

        public static void LogError(params object[] args)
        {
            LogOut(args, log.LogError);
            //ConsoleOut(args, ConsoleColor.DarkRed);
        }

    }
}
