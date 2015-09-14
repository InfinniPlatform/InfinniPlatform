using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace InfinniPlatform.UserInterface.ViewBuilders.Images
{
    /// <summary>
    ///     Преобразует строку в изображение.
    /// </summary>
    [ValueConversion(typeof (string), typeof (ImageSource))]
    internal sealed class ImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ImageRepository.GetImage(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}