using System;

namespace InfinniPlatform.PrintView.Factories.DisplayFormats
{
    internal class NumberFormatFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            return FormatFunc(elementMetadata.Format);
        }

        private static Func<object, string> FormatFunc(object format)
        {
            var formatString = format as string;

            if (string.IsNullOrEmpty(formatString))
            {
                formatString = "n";
            }

            return value =>
            {
                double valueDouble;

                if (ConvertHelper.TryToDouble(value, out valueDouble))
                {
                    return valueDouble.ToString(formatString);
                }

                return null;
            };
        }
    }
}