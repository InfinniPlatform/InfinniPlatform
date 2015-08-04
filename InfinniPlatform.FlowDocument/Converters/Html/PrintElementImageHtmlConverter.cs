using System.Text;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementImageHtmlConverter : IHtmlBuilderBase<PrintElementImage>
    {
        public override void Build(HtmlBuilderContext context, PrintElementImage element, StringBuilder result)
        {
            result.Append("<img src=\"data:image/png;base64,")
                .StreamToBase64(element.Source)
                .Append("\" style=\"")
                .ApplyBaseStyles(element)
                .ApplyInlineStyles(element)
                .ApplyImageStyles(element)
                .Append("\">")
                .Append("</img>");
        }
    }
}
