using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    internal sealed class FileHistoryHelper
    {
        private static readonly ConcurrentDictionary<string, DateTime> History = new ConcurrentDictionary<string, DateTime>();

        public static bool IsChanged(string filePath)
        {
            var lastWriteTime = File.GetLastWriteTime(filePath);

            //В какой-то момент File.GetLastWriteTime() возвращает дату 01.01.1601, что соответствует удаленному файлу,
            //хотя на самом деле файл был отредактирован. В результате экспериментов выяснилось,
            //что после задержки (в дебаге или с использованием Thread.Sleep()) File.GetLastWriteTime() возвращает корректное значение.
            if (lastWriteTime == DateTime.FromFileTime(0))
            {
                Thread.Sleep(500);
                lastWriteTime = File.GetLastWriteTime(filePath);
            }

            DateTime lastReadTime;

            if (History.TryGetValue(filePath, out lastReadTime))
            {
                if (EqualsToSeconds(lastWriteTime, lastReadTime))
                {
                    return false;
                }

                History.TryUpdate(filePath, lastWriteTime, lastReadTime);

                return true;
            }

            History.TryAdd(filePath, lastWriteTime);

            return true;
        }

        private static bool EqualsToSeconds(DateTime left, DateTime right)
        {
            return left.Year == right.Year &&
                   left.Month == right.Month &&
                   left.Day == right.Day &&
                   left.Minute == right.Minute &&
                   left.Second == right.Second;
        }
    }
}