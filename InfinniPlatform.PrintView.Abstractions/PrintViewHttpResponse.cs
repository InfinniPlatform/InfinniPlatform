using System;
using System.IO;

using InfinniPlatform.Core.Http;
using InfinniPlatform.Core.Threading;

namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Ответ в виде печатного представления.
    /// </summary>
    public class PrintViewHttpResponse : HttpResponse
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="builder">Фабрика печатных представлений.</param>
        /// <param name="template">Шаблон печатного представления.</param>
        /// <param name="dataSource">Данные печатного представления.</param>
        /// <param name="fileFormat">Формат файла печатного представления.</param>
        public PrintViewHttpResponse(IPrintViewBuilder builder,
                                     Func<Stream> template,
                                     object dataSource = null,
                                     PrintViewFileFormat fileFormat = PrintViewFileFormat.Pdf)
        {
            ContentType = GetPrintViewContentType(fileFormat);

            Content = stream => AsyncHelper.RunSync(() => builder.Build(stream, template, dataSource, fileFormat));
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="builder">Фабрика печатных представлений.</param>
        /// <param name="template">Шаблон печатного представления.</param>
        /// <param name="dataSource">Данные печатного представления.</param>
        /// <param name="fileFormat">Формат файла печатного представления.</param>
        public PrintViewHttpResponse(IPrintViewBuilder builder,
                                     PrintDocument template,
                                     object dataSource = null,
                                     PrintViewFileFormat fileFormat = PrintViewFileFormat.Pdf)
        {
            ContentType = GetPrintViewContentType(fileFormat);

            Content = stream => AsyncHelper.RunSync(() => builder.Build(stream, template, dataSource, fileFormat));
        }


        private static string GetPrintViewContentType(PrintViewFileFormat fileFormat)
        {
            switch (fileFormat)
            {
                case PrintViewFileFormat.Pdf:
                    return HttpConstants.PdfContentType;
                case PrintViewFileFormat.Html:
                    return HttpConstants.HtmlContentType;
            }

            return HttpConstants.StreamContentType;
        }
    }
}