using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

using InfinniPlatform.Api.Extensions;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    sealed class FlowElementImageBuilder : IFlowElementBuilderBase<PrintElementImage>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementImage element)
        {
            var elementContent = new InlineUIContainer();

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyInlineStyles(elementContent, element);

            var imageSource = new BitmapImage {StreamSource = element.Source};

            var imageControl = new Image
            {
                Width = element.Size.Maybe(s => s.Width) ?? imageSource.Width,
                Height = element.Size.Maybe(s => s.Height) ?? imageSource.Height,
                Source = imageSource
            };

            elementContent.Child = imageControl;

            return elementContent;
        }
    }
}
