using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Controls
{
	[ValueConversion(typeof(IEnumerable), typeof(Visibility))]
	sealed class VisibilityCommandsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var commands = value as IEnumerable;

			if (commands != null)
			{
				if (commands.Cast<object>().Any())
				{
					return Visibility.Visible;
				}
			}

			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}