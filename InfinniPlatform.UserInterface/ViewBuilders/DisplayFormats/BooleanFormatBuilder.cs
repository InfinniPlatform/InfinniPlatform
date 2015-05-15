using System;

using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DisplayFormats
{
	sealed class BooleanFormatBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			return new DataFormat(FormatFunc(metadata.TrueText, metadata.FalseText));
		}

		private static Func<object, string> FormatFunc(object trueText, object falseText)
		{
			var trueTextString = string.IsNullOrEmpty(trueText as string) ? true.ToString() : (trueText as string);
			var falseTextString = string.IsNullOrEmpty(falseText as string) ? false.ToString() : (falseText as string);

			return value =>
				   {
					   bool valueBool;

					   if (TryToBool(value, out valueBool))
					   {
						   return valueBool ? trueTextString : falseTextString;
					   }

					   return null;
				   };
		}

		private static bool TryToBool(object value, out bool result)
		{
			result = default(bool);

			if (value != null)
			{
				try
				{
					result = Convert.ToBoolean(value);
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