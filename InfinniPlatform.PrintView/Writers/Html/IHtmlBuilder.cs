using System.IO;

using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal interface IHtmlBuilder
    {
        void Build(HtmlBuilderContext context, PrintElement element, TextWriter result);
    }
}