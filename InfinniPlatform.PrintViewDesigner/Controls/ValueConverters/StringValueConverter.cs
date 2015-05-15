﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace InfinniPlatform.PrintViewDesigner.Controls.ValueConverters
{
	[ValueConversion(typeof(object), typeof(string))]
	sealed class StringValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = null;

			if (value != null)
			{
				try
				{
					result = System.Convert.ToString(value, culture);
				}
				catch
				{
				}
			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = null;

			if (value != null)
			{
				try
				{
					result = System.Convert.ToString(value, culture);
				}
				catch
				{
				}
			}

			return result;
		}
	}
}