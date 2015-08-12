using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using InfinniPlatform.Api.Extensions;
using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    sealed class FlowElementImageBuilder : IFlowElementBuilderBase<PrintElementImage>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementImage element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new InlineUIContainer { BaselineAlignment = BaselineAlignment.Baseline };

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyInlineStyles(elementContent, element);

            var imageSource = new BitmapImage();

            imageSource.BeginInit();
            imageSource.StreamSource = element.Source;
            imageSource.EndInit();

            var imageControl = new Image();

            imageControl.BeginInit();
            imageControl.Width = element.Size.Maybe(s => s.Width) ?? imageSource.Width;
            imageControl.Height = element.Size.Maybe(s => s.Height) ?? imageSource.Height;
            imageControl.Source = imageSource;
            imageControl.Stretch = Stretch.Fill;
            imageControl.EndInit();

            elementContent.Child = imageControl;

            return elementContent;
        }
    }
}
