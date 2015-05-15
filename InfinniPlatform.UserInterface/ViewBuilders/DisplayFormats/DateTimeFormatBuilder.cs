using System;

using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DisplayFormats
{
	sealed class DateTimeFormatBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			return new DataFormat(FormatFunc(metadata.Format));
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

					   if (TryToDateTime(value, out valueDateTime))
					   {
						   return valueDateTime.ToString(formatString);
					   }

					   return null;
				   };
		}

		private static bool TryToDateTime(object value, out DateTime result)
		{
			result = default(DateTime);

			if (value != null)
			{
				try
				{
					result = Convert.ToDateTime(value);
					return true;
				}
				catch
				{
				}
			}

			return false;
		}
	}
}