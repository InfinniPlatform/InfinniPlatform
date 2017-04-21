using System.IO;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal interface IHtmlBuilder
    {
        void Build(HtmlBuilderContext context, PrintElement element, TextWriter result);
    }
}