using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Factories;
using InfinniPlatform.PrintView.Writers.Html;
using InfinniPlatform.PrintView.Writers.Pdf;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Contract
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintViewBuilderTest
    {
        [Test]
        [TestCase(PrintViewFileFormat.Pdf)]
        [TestCase(PrintViewFileFormat.Html)]
        public async Task ShouldBuildFile(PrintViewFileFormat fileFormat)
        {
            // Given
            var target = CreatePrintViewBuilder();
            var template = GetPrintViewExample();
            var dataSource = new DynamicWrapper { { "Date", DateTime.Now } };

            // When
            var result = new MemoryStream();
            await target.Build(result, template, dataSource, fileFormat);

            // Then
            Assert.IsNotNull(result);
            Assert.Greater(result.Length, 0);
        }

        [Test]
        [Ignore("Manual")]
        [TestCase(PrintViewFileFormat.Pdf)]
        [TestCase(PrintViewFileFormat.Html)]
        public async Task ShouldBuildFileAndThenOpenIt(PrintViewFileFormat fileFormat)
        {
            // Given
            var target = CreatePrintViewBuilder();
            var template = GetPrintViewExample();
            var dataSource = new DynamicWrapper { { "Date", DateTime.Now } };

            // When
            var result = new MemoryStream();
            await target.Build(result, template, dataSource, fileFormat);

            // Then
            Assert.IsNotNull(result);
            Assert.Greater(result.Length, 0);

            // Open PrintView

            result.Position = 0;

            await OpenPrintView(result, fileFormat);
        }


        private static PrintViewBuilder CreatePrintViewBuilder()
        {
            var printViewSerializer = new PrintViewSerializer();

            var printDocumentBuilder = new PrintDocumentBuilder();

            var printViewWriter = new PrintViewWriter();
            var printViewSettings = PrintViewSettings.Default;
            var htmlPrintDocumentWriter = new HtmlPrintDocumentWriter();
            var pdfPrintDocumentWriter = new PdfPrintDocumentWriter(printViewSettings, htmlPrintDocumentWriter);
            printViewWriter.RegisterWriter(PrintViewFileFormat.Html, htmlPrintDocumentWriter);
            printViewWriter.RegisterWriter(PrintViewFileFormat.Pdf, pdfPrintDocumentWriter);

            return new PrintViewBuilder(printViewSerializer, printDocumentBuilder, printViewWriter);
        }

        private static Func<Stream> GetPrintViewExample()
        {
            var test = typeof(PrintViewBuilderTest);

            return () => test.Assembly.GetManifestResourceStream(test, "PrintViewExample.json");
        }

        private static async Task OpenPrintView(Stream printView, PrintViewFileFormat fileFormat)
        {
            var fileName = "PrintView." + fileFormat;

            using (var writer = File.Create(fileName))
            {
                await printView.CopyToAsync(writer);
                writer.Flush();
                writer.Close();
            }

            Process.Start(fileName);
        }
    }
}