using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DevExpress.Xpf.Core;

namespace InfinniPlatform.UserInterface.ViewBuilders.Themes
{
    /// <summary>
    ///     Настройки темы элементов управления.
    /// </summary>
    internal sealed class ThemeSettings : INotifyPropertyChanged
    {
        private static ThemeSettings _instance;
        private Brush _borderBrush;
        private Thickness _borderThickness;

        public ThemeSettings()
        {
            ThemeManager.ThemeChanged += OnThemeChanged;

            SetProperties(ThemeManager.ActualApplicationThemeName);
        }

        public static ThemeSettings Instance
        {
            get { return _instance ?? (_instance = new ThemeSettings()); }
        }

        /// <summary>
        ///     Цвета границ.
        /// </summary>
        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set
            {
                if (Equals(_borderBrush, value) == false)
                {
                    _borderBrush = value;

                    InvokePropertyChanged("BorderBrush");
                }
            }
        }

        /// <summary>
        ///     Контуры границ.
        /// </summary>
        public Thickness BorderThickness
        {
            get { return _borderThickness; }
            set
            {
                if (_borderThickness != value)
                {
                    _borderThickness = value;

                    InvokePropertyChanged("BorderThickness");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string property)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        public static void OnThemeChanged(DependencyObject sender, ThemeChangedRoutedEventArgs e)
        {
            Instance.SetProperties(e.ThemeName);
        }

        private void SetProperties(string name)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;

            // Создаем стилизованный элемент
            var element = new ListBox();
            ThemeManager.SetThemeName(element, name);

            // Считываем стилизованные значения свойств
            BorderBrush = element.BorderBrush;
            BorderThickness = element.BorderThickness;

            ThemeManager.ThemeChanged += OnThemeChanged;
        }
    }
}