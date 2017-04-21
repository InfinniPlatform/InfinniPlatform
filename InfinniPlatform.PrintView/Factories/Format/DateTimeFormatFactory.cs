using System;

using InfinniPlatform.PrintView.Defaults;
using InfinniPlatform.PrintView.Format;

namespace InfinniPlatform.PrintView.Factories.Format
{
    internal class DateTimeFormatFactory : PrintElementFactoryBase<DateTimeFormat>
    {
        public override object Create(PrintElementFactoryContext context, DateTimeFormat template)
        {
            return FormatFunc(template.Format);
        }

        private static Func<object, string> FormatFunc(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = PrintViewDefaults.Formats.DateTimeFormat;
            }

            return value =>
                   {
                       DateTime valueDateTime;

                       if (ConvertHelper.TryToDateTime(value, out valueDateTime))
                       {
                           return valueDateTime.ToString(format);
                       }

                       return null;
                   };
        }
    }
}