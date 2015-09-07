﻿using InfinniPlatform.FlowDocument.Model.Font;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public sealed class PrintElementTableCell
    {
        public string Name { get; set; }
        public string Style { get; set; }
        public PrintElementFont Font { get; set; }
        public string Foreground { get; set; }
        public string Background { get; set; }
        public PrintElementBorder Border { get; set; }
        public PrintElementThickness Padding { get; set; }
        public PrintElementTextAlignment? TextAlignment { get; set; }
        public int? ColumnSpan { get; set; }
        public int? RowSpan { get; set; }
        public PrintElementBlock Block { get; set; }
    }
}