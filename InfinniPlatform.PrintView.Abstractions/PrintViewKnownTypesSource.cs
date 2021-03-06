﻿using InfinniPlatform.PrintView.Block;
using InfinniPlatform.PrintView.Format;
using InfinniPlatform.PrintView.Inline;
using InfinniPlatform.Serialization;

namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Источник известных типов для <see cref="PrintDocument" />.
    /// </summary>
    public class PrintViewKnownTypesSource : IKnownTypesSource
    {
        /// <inheritdoc />
        public void AddKnownTypes(KnownTypesContainer knownTypesContainer)
        {
            knownTypesContainer
                // Format
                .Add<BooleanFormat>(BooleanFormat.TypeName)
                .Add<DateTimeFormat>(DateTimeFormat.TypeName)
                .Add<NumberFormat>(NumberFormat.TypeName)
                .Add<ObjectFormat>(ObjectFormat.TypeName)
                // Block
                .Add<PrintLine>(PrintLine.TypeName)
                .Add<PrintList>(PrintList.TypeName)
                .Add<PrintPageBreak>(PrintPageBreak.TypeName)
                .Add<PrintParagraph>(PrintParagraph.TypeName)
                .Add<PrintSection>(PrintSection.TypeName)
                .Add<PrintTable>(PrintTable.TypeName)
                // Inline
                .Add<PrintBarcodeEan13>(PrintBarcodeEan13.TypeName)
                .Add<PrintBarcodeQr>(PrintBarcodeQr.TypeName)
                .Add<PrintBold>(PrintBold.TypeName)
                .Add<PrintHyperlink>(PrintHyperlink.TypeName)
                .Add<PrintImage>(PrintImage.TypeName)
                .Add<PrintItalic>(PrintItalic.TypeName)
                .Add<PrintLineBreak>(PrintLineBreak.TypeName)
                .Add<PrintRun>(PrintRun.TypeName)
                .Add<PrintSpan>(PrintSpan.TypeName)
                .Add<PrintUnderline>(PrintUnderline.TypeName);
        }
    }
}