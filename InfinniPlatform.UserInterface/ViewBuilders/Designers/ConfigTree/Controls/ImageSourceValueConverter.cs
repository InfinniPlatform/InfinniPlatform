using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using InfinniPlatform.UserInterface.ViewBuilders.Images;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Controls
{
	[ValueConversion(typeof(string), typeof(ImageSource))]
	sealed class ImageSourceValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value is string) ? ImageRepository.GetImage(value as string) : value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}