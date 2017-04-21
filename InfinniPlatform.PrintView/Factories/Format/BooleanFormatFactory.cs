using System;

using InfinniPlatform.PrintView.Defaults;
using InfinniPlatform.PrintView.Format;

namespace InfinniPlatform.PrintView.Factories.Format
{
    internal class BooleanFormatFactory : PrintElementFactoryBase<BooleanFormat>
    {
        public override object Create(PrintElementFactoryContext context, BooleanFormat template)
        {
            return FormatFunc(template.TrueText, template.FalseText);
        }

        private static Func<object, string> FormatFunc(string trueText, string falseText)
        {
            if (string.IsNullOrEmpty(trueText))
            {
                trueText = PrintViewDefaults.Formats.TrueText;
            }

            if (string.IsNullOrEmpty(falseText))
            {
                falseText = PrintViewDefaults.Formats.FalseText;
            }

            return value =>
                   {
                       bool valueBool;

                       if (ConvertHelper.TryToBool(value, out valueBool))
                       {
                           return valueBool ? trueText : falseText;
                       }

                       return null;
                   };
        }
    }
}