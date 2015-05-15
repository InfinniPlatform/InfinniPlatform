using System;
using System.Collections.Generic;

namespace InfinniPlatform.Api.Registers
{
    /// <summary>
    /// Периодичность записи в регистр
    /// </summary>
    public static class RegisterPeriod
    {
        public const string Second = "Second";
        public const string Minute = "Minute";
        public const string Hour = "Hour";
        public const string Day = "Day";
        public const string Month = "Month";
        public const string Quarter = "Quarter";
        public const string HalfYear = "HalfYear";
        public const string Year = "Year";
        

        public static IEnumerable<string> GetRegisterPeriods()
        {
            return new[]
	        {
	            Second, 
                Minute, 
                Hour,
                Day,
                Month,
                Quarter,
                HalfYear,
                Year
	        };
        }

        /// <summary>
        /// Подстраивает дату документа под период
        /// </summary>
        public static DateTime AdjustDateToPeriod(DateTime sourceDateTime, string period)
        {
            var documentDate = sourceDateTime;

            if (!string.IsNullOrEmpty(period))
            {
                switch (period)
                {
                    case Year:
                        documentDate = new DateTime(sourceDateTime.Year, 1, 1);
                        break;
                    case HalfYear:
                        documentDate = new DateTime(sourceDateTime.Year, (sourceDateTime.Month - 1)/6*6 + 1, 1);
                        break;
                    case Quarter:
                        documentDate = new DateTime(sourceDateTime.Year, (sourceDateTime.Month - 1)/3*3 + 1, 1);
                        break;
                    case Month:
                        documentDate = new DateTime(sourceDateTime.Year, sourceDateTime.Month, 1);
                        break;
                    case Day:
                        documentDate = new DateTime(sourceDateTime.Year, sourceDateTime.Month, sourceDateTime.Day);
                        break;
                    case Hour:
                        documentDate = new DateTime(sourceDateTime.Year, sourceDateTime.Month, sourceDateTime.Day,
                            sourceDateTime.Hour, 0, 0);
                        break;
                    case Minute:
                        documentDate = new DateTime(sourceDateTime.Year, sourceDateTime.Month, sourceDateTime.Day,
                            sourceDateTime.Hour, sourceDateTime.Minute, 0);
                        break;
                    case Second:
                        documentDate = new DateTime(sourceDateTime.Year, sourceDateTime.Month, sourceDateTime.Day,
                            sourceDateTime.Hour, sourceDateTime.Minute, sourceDateTime.Second);
                        break;
                }
            }

            return documentDate;
        }
    }
}
