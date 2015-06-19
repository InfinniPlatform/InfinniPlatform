using System;

namespace InfinniPlatform.FlowDocument.Builders.Factories.DisplayFormats
{
    internal sealed class DateTimeFormatFactory : IPrintElementFactory
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
                formatString = "G";
            }

            return value =>
            {
                DateTime valueDateTime;

                if (ConvertHelper.TryToDateTime(value, out valueDateTime))
                {
                    return valueDateTime.ToString(formatString);
                }

                return null;
            };
        }
    }
}