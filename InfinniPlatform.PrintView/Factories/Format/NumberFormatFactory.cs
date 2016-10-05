using System;

using InfinniPlatform.PrintView.Model.Defaults;
using InfinniPlatform.PrintView.Model.Format;

namespace InfinniPlatform.PrintView.Factories.Format
{
    internal class NumberFormatFactory : PrintElementFactoryBase<NumberFormat>
    {
        public override object Create(PrintElementFactoryContext context, NumberFormat template)
        {
            return FormatFunc(template.Format);
        }

        private static Func<object, string> FormatFunc(string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = PrintViewDefaults.Formats.NumberFormat;
            }

            return value =>
                   {
                       double valueDouble;

                       if (ConvertHelper.TryToDouble(value, out valueDouble))
                       {
                           return valueDouble.ToString(format);
                       }

                       return null;
                   };
        }
    }
}