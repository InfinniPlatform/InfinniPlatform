using System;
using System.Collections.Concurrent;
using System.IO;

namespace InfinniPlatform.SystemConfig.Initializers
{
    internal sealed class FileHistoryHelper
    {
        readonly static ConcurrentDictionary<string, DateTime> History = new ConcurrentDictionary<string, DateTime>();

        public static bool IsChanged(string filePath)
        {
            var lastWriteTime = File.GetLastWriteTime(filePath);

            DateTime lastReadTime;

            if (History.TryGetValue(filePath, out lastReadTime))
            {
                if (!EqualsToSeconds(lastWriteTime, lastReadTime))
                {
                    History.TryUpdate(filePath, lastWriteTime, lastReadTime);

                    return true;
                }
            }
            else
            {
                History.TryAdd(filePath, lastWriteTime);

                return true;
            }

            return false;
        }

        static bool EqualsToSeconds(DateTime left, DateTime right)
        {
            return left.Year == right.Year &&
                   left.Month == right.Month &&
                   left.Day == right.Day &&
                   left.Minute == right.Minute &&
                   left.Second == right.Second;

        }
    }
}