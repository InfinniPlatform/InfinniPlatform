using System.IO;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Format;
using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.PrintView.Contract
{
    /// <summary>
    /// Сериализатор для <see cref="PrintDocument" />.
    /// </summary>
    public class PrintViewSerializer : IPrintViewSerializer
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintViewSerializer()
        {
            var knownTypes = new KnownTypesContainer()
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

            _serializer = new JsonObjectSerializer(true, knownTypes: knownTypes);
        }


        private readonly IJsonObjectSerializer _serializer;


        /// <summary>
        /// Сериализует документ.
        /// </summary>
        /// <param name="stream">Поток для записи.</param>
        /// <param name="document">Документ печатного представления.</param>
        public void Serialize(Stream stream, PrintDocument document)
        {
            _serializer.Serialize(stream, document);
        }

        /// <summary>
        /// Десериализует документ.
        /// </summary>
        /// <param name="stream">Поток для чтения.</param>
        /// <returns>Документ печатного представления.</returns>
        public PrintDocument Deserialize(Stream stream)
        {
            return _serializer.Deserialize<PrintDocument>(stream);
        }
    }
}