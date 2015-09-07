using System.IO;

using InfinniPlatform.FlowDocument.Model;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    public abstract class IHtmlBuilderBase<TElement> : IHtmlBuilder where TElement : PrintElement
    {
        public void Build(HtmlBuilderContext context, PrintElement element, TextWriter result)
        {
            Build(context, (TElement)element, result);
        }

        public abstract void Build(HtmlBuilderContext context, TElement element, TextWriter result);
    }
}