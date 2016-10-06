using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.PrintView.Contract
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

            Content = stream => RunSync(() => builder.Build(stream, template, dataSource, fileFormat));
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

            Content = stream => RunSync(() => builder.Build(stream, template, dataSource, fileFormat));
        }


        private static readonly TaskFactory InternalTaskFactory
            = new TaskFactory(CancellationToken.None,
                              TaskCreationOptions.None,
                              TaskContinuationOptions.None,
                              TaskScheduler.Default);

        private static void RunSync(Func<Task> func)
        {
            var culture = CultureInfo.CurrentCulture;
            var cultureUi = CultureInfo.CurrentUICulture;

            var syncTask = InternalTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            });

            syncTask.Unwrap().GetAwaiter().GetResult();
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