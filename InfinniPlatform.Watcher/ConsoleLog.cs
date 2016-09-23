using System;

namespace InfinniPlatform.Watcher
{
    public static class ConsoleLog
    {
        public static void Info(string s)
        {
            Console.WriteLine(s);
        }

        public static void Error(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        public static void Error(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ResetColor();
        }

        public static void Warning(string s)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(s);
            Console.ResetColor();
        }
    }
}