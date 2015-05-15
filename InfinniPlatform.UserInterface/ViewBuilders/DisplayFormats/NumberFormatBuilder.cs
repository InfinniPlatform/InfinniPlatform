using System;

using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DisplayFormats
{
	sealed class NumberFormatBuilder : IObjectBuilder
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
				formatString = "n";
			}

			return value =>
				   {
					   double valueDouble;

					   if (TryToDouble(value, out valueDouble))
					   {
						   return valueDouble.ToString(formatString);
					   }

					   return null;
				   };
		}

		private static bool TryToDouble(object value, out double result)
		{
			result = default(double);

			if (value != null)
			{
				try
				{
					result = Convert.ToDouble(value);
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