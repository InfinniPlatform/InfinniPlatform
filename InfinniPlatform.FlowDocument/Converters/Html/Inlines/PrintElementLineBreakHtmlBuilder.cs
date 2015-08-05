﻿using System.IO;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html.Inlines
{
    class PrintElementLineBreakHtmlBuilder : IHtmlBuilderBase<PrintElementLineBreak>
    {
        public override void Build(HtmlBuilderContext context, PrintElementLineBreak element, TextWriter result)
        {
            result.Write("<br>");
        }
    }
}
