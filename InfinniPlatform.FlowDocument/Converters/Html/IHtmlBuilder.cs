using System.Text;

using InfinniPlatform.FlowDocument.Model;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    public interface IHtmlBuilder
    {
        void Build(HtmlBuilderContext context, PrintElement element, StringBuilder result);
    }
}