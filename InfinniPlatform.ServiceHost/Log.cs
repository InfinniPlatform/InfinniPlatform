using System;
using System.IO;

namespace InfinniPlatform.ServiceHost
{
    public static class Log
    {
        private const string LogFileName = "startLog.txt";

        public static void Clear()
        {
            File.Delete(LogFileName);
        }

        public static void Add(string s)
        {
            File.AppendAllText(LogFileName, $"{s}{Environment.NewLine}");
        }

        public static void Add(object s)
        {
            Add(s.ToString());
        }
    }
}