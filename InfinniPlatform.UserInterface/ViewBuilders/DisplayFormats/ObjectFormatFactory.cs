using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DisplayFormats
{
	sealed class ObjectFormatBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			return new DataFormat(FormatFunc(metadata.Format));
		}

		private static readonly object[] EmptyArray = { };

		private static Func<object, string> FormatFunc(object format)
		{
			string formatString;
			ICollection<string> formatProperties;
			BuildFormat(format as string, out formatString, out formatProperties);

			return value =>
				   {
					   if (value != null && string.IsNullOrEmpty(formatString) == false)
					   {
						   object[] properties = (formatProperties != null) ? formatProperties.Select(value.GetProperty).ToArray() : EmptyArray;
						   return string.Format(formatString, properties);
					   }

					   return null;
				   };
		}

		private static void BuildFormat(string format, out string formatString, out ICollection<string> formatProperties)
		{
			formatString = null;
			formatProperties = new List<string>();

			if (string.IsNullOrEmpty(format) == false)
			{
				var index = 0;
				var brackets = 0;

				var isProperty = false;
				var isPropertyFormat = false;

				var propertyNameBuilder = new StringBuilder();
				var propertyFormatBuilder = new StringBuilder();
				var formatStringBuilder = new StringBuilder();

				foreach (char c in format)
				{
					if (c == '{')
					{
						++brackets;

						// Начало выражения форматирования свойства
						if (brackets == 1)
						{
							isProperty = true;
							isPropertyFormat = false;

							propertyNameBuilder.Clear();
							propertyFormatBuilder.Clear();

							continue;
						}
					}
					else if (c == ':')
					{
						// Начало настроек форматирования свойства
						if (isProperty)
						{
							isProperty = false;
							isPropertyFormat = true;

							continue;
						}
					}
					else if (c == '}')
					{
						--brackets;

						// Конец выражения форматирования свойства
						if (brackets == 0)
						{
							var propertyName = propertyNameBuilder.ToString();
							formatProperties.Add(propertyName);

							formatStringBuilder.Append('{').Append(index++);

							if (isPropertyFormat)
							{
								var propertyFormat = propertyFormatBuilder.ToString();
								formatStringBuilder.Append(':').Append(propertyFormat);
							}

							formatStringBuilder.Append('}');

							isProperty = false;
							isPropertyFormat = false;

							continue;
						}
					}

					if (isProperty)
					{
						propertyNameBuilder.Append(c);
					}
					else if (isPropertyFormat)
					{
						propertyFormatBuilder.Append(c);
					}
					else
					{
						formatStringBuilder.Append(c);
					}
				}

				formatString = formatStringBuilder.ToString();
			}
		}
	}
}