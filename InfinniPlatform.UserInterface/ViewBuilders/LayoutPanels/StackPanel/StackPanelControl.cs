using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.StackPanel
{
    /// <summary>
    ///     Элемент управления для контейнера элементов представления в виде стека.
    /// </summary>
    public sealed class StackPanelControl : Grid
    {
        private bool _arrangeChildren;

        public StackPanelControl()
        {
            Loaded += (s, e) => ArrangeChildren();
        }

        /// <summary>
        ///     Ориентация стека.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        private static void OnVisualPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StackPanelControl;

            if (control != null)
            {
                control.ArrangeChildren();
            }
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            ArrangeChildren();
        }

        private void ArrangeChildren()
        {
            if (_arrangeChildren == false)
            {
                _arrangeChildren = true;

                RowDefinitions.Clear();
                ColumnDefinitions.Clear();

                var children = Children.Cast<FrameworkElement>().ToArray();

                if (Orientation == Orientation.Vertical)
                {
                    var index = 0;
                    var rows = RowDefinitions;

                    foreach (var child in children)
                    {
                        rows.Add(new RowDefinition
                        {
                            Height = CalcChildLength(child, child.Height),
                            MinHeight = child.MinHeight,
                            MaxHeight = child.MaxHeight
                        });

                        SetRow(child, index);
                        SetColumn(child, 0);

                        ++index;
                    }
                }
                else
                {
                    var index = 0;
                    var columns = ColumnDefinitions;

                    foreach (var child in children)
                    {
                        columns.Add(new ColumnDefinition
                        {
                            Width = CalcChildLength(child, child.Width),
                            MinWidth = child.MinWidth,
                            MaxWidth = child.MaxWidth
                        });

                        SetRow(child, 0);
                        SetColumn(child, index);

                        ++index;
                    }
                }

                _arrangeChildren = false;
            }
        }

        private static GridLength CalcChildLength(FrameworkElement child, double size)
        {
            // Todo: Эту логику еще нужно хорошенько отладить

            if (double.IsNaN(size) == false)
            {
                return new GridLength(1, GridUnitType.Auto);
            }

            if (child is StackPanelControl)
            {
                return new GridLength(1, GridUnitType.Star);
            }

            return new GridLength(1, GridUnitType.Star);

            //return (double.IsNaN(size) == false || child is StackPanelControl)
            //	? new GridLength(1, GridUnitType.Auto)
            //	: new GridLength(1, GridUnitType.Star);
        }

        // Orientation

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof (Orientation), typeof (StackPanelControl),
            new FrameworkPropertyMetadata(Orientation.Vertical, OnVisualPropertyChanged));
    }
}