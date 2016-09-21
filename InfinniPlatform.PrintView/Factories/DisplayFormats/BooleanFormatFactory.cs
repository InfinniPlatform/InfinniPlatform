using System;

namespace InfinniPlatform.PrintView.Factories.DisplayFormats
{
    internal class BooleanFormatFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            return FormatFunc(elementMetadata.TrueText, elementMetadata.FalseText);
        }

        private static Func<object, string> FormatFunc(object trueText, object falseText)
        {
            var trueTextString = string.IsNullOrEmpty(trueText as string) ? true.ToString() : (trueText as string);
            var falseTextString = string.IsNullOrEmpty(falseText as string) ? false.ToString() : (falseText as string);

            return value =>
            {
                bool valueBool;

                if (ConvertHelper.TryToBool(value, out valueBool))
                {
                    return valueBool ? trueTextString : falseTextString;
                }

                return null;
            };
        }
    }
}