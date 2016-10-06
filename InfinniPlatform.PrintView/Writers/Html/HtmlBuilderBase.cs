using System.IO;

using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal abstract class HtmlBuilderBase<TElement> : IHtmlBuilder where TElement : PrintElement
    {
        public void Build(HtmlBuilderContext context, PrintElement element, TextWriter result)
        {
            Build(context, (TElement)element, result);
        }

        public abstract void Build(HtmlBuilderContext context, TElement element, TextWriter result);
    }
}