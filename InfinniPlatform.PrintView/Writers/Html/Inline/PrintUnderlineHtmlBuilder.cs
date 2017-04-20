﻿using System.IO;

using InfinniPlatform.PrintView.Inline;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    internal class PrintUnderlineHtmlBuilder : HtmlBuilderBase<PrintUnderline>
    {
        public override void Build(HtmlBuilderContext context, PrintUnderline element, TextWriter result)
        {
            result.Write("<ins style=\"");
            result.ApplyElementStyles(element);
            result.ApplyInlineStyles(element);
            result.Write("\">");

            result.ApplySubOrSup(element);

            if (element.Inlines != null)
            {
                foreach (var item in element.Inlines)
                {
                    context.Build(item, result);
                }
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</ins>");
        }
    }
}