using System;
using System.Drawing;
using System.IO;

using FastReport;
using FastReport.Table;
using FastReport.Utils;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.Serialization;
using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Font;
using InfinniPlatform.FastReport.Templates.Reports;
using InfinniPlatform.FastReport.TemplatesFluent.Data;
using InfinniPlatform.FastReport.TemplatesFluent.Print;
using InfinniPlatform.FastReport.TemplatesFluent.Reports;
using InfinniPlatform.FastReport.Tests.TestEntities;

using NUnit.Framework;

using FRReport = FastReport.Report;
using TableRow = FastReport.Table.TableRow;
using TableColumn = FastReport.Table.TableColumn;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Behaviors
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class FastReportBehaviorTest
    {
        [Test]
        [Ignore]
        public void CreateReport()
        {
            var template = CreateReportTemplateConfig()
                .DataSources(ds => ds
                    .Register("Document", i => i
                        .Schema(s => s
                            .Property("Patient", p => p
                                .Property("FirstName", SchemaDataType.String)
                                .Property("MiddleName", SchemaDataType.String)
                                .Property("LastName", SchemaDataType.String))
                            .Property("Diagnosis", p => p
                                .Property("Code", SchemaDataType.String)
                                .Property("DisplayName", SchemaDataType.String)
                                .Property("CodeSystem", SchemaDataType.String)))
                        .Provider(p => p.Rest(r => r
                            .RequestUri(@"http://localhost:12345/api/Document")
                            .ContentType("application/json")
                            .Method("GET")))))
                .Totals("DocumentDataBand", t => t
                    .Count("CountByDiagnosis", "DiagnosisGroupBand"))
                .Page(p => p
                    .Header(h => h.CanGrow().Height(10).Elements(e => e
                        .Table(t => t.Layout(l => l.Left(0).Top(0).Height(10))
                            .Grid(
                                rows => rows
                                    .Row(r => r.Height(10)),
                                columns => columns
                                    .Column(c => c.Width(40))
                                    .Column(c => c.Width(40))
                                    .Column(c => c.Width(40)),
                                cells => cells
                                    .Cell(0, 0, c => c
                                        .Bind(b => b.Constant("Фамилия"))
                                        .Border(b => b.All(l => l.Color("Black").Width(1).Style(BorderLineStyle.Solid)))
                                        .Style(s => s.Foreground("Black").Background("LightGray").HorizontalAlignment(HorizontalAlignment.Center).VerticalAlignment(VerticalAlignment.Center).Font("Arial", 10, options: f => f.FontWeight(FontWeight.Bold)).WordWrap()))
                                    .Cell(0, 1, c => c
                                        .Bind(b => b.Constant("Имя"))
                                        .Border(b => b.All(l => l.Color("Black").Width(1).Style(BorderLineStyle.Solid)))
                                        .Style(s => s.Foreground("Black").Background("LightGray").HorizontalAlignment(HorizontalAlignment.Center).VerticalAlignment(VerticalAlignment.Center).Font("Arial", 10, options: f => f.FontWeight(FontWeight.Bold)).WordWrap()))
                                    .Cell(0, 2, c => c
                                        .Bind(b => b.Constant("Отчество"))
                                        .Border(b => b.All(l => l.Color("Black").Width(1).Style(BorderLineStyle.Solid)))
                                        .Style(s => s.Foreground("Black").Background("LightGray").HorizontalAlignment(HorizontalAlignment.Center).VerticalAlignment(VerticalAlignment.Center).Font("Arial", 10, options: f => f.FontWeight(FontWeight.Bold)).WordWrap())))))))
                .Data("Document", d => d
                    .OrderBy("Patient.LastName")
                    .OrderBy("Patient.FirstName")
                    .OrderBy("Patient.MiddleName")
                    .Group("Diagnosis.DisplayName", g => g
                        .Header(h => h.CanGrow().Height(10).Elements(e => e
                            .Text(t => t.CanGrow()
                                .Layout(l => l.Left(0).Top(0).Height(10).Width(120))
                                .Bind(b => b.Property("Document", "Diagnosis.DisplayName"))
                                .Border(b => b.All(l => l.Color("Black").Width(1).Style(BorderLineStyle.Solid)))
                                .Style(s => s.Foreground("Black").Background("White").HorizontalAlignment(HorizontalAlignment.Left).VerticalAlignment(VerticalAlignment.Center).Font("Arial", 8, options: f => f.FontWeight(FontWeight.Bold)).WordWrap()))))
                        .Footer(h => h.Name("DiagnosisGroupBand").CanGrow().Height(10).Elements(e => e
                            .Text(t => t
                                .Layout(l => l.Left(0).Top(0).Height(10).Width(80))
                                .Bind(b => b.Constant("Всего с таким диагнозом:"))
                                .Border(b => b.All(l => l.Color("Black").Width(1).Style(BorderLineStyle.Solid)))
                                .Style(s => s.Foreground("Black").Background("White").HorizontalAlignment(HorizontalAlignment.Left).VerticalAlignment(VerticalAlignment.Center).Font("Arial", 8, options: f => f.FontWeight(FontWeight.Bold)).WordWrap()))
                            .Text(t => t
                                .Layout(l => l.Left(80).Top(0).Height(10).Width(40))
                                .Bind(b => b.Total("CountByDiagnosis"))
                                .Border(b => b.All(l => l.Color("Black").Width(1).Style(BorderLineStyle.Solid)))
                                .Style(s => s.Foreground("Black").Background("White").HorizontalAlignment(HorizontalAlignment.Center).VerticalAlignment(VerticalAlignment.Center).Font("Arial", 8, options: f => f.FontWeight(FontWeight.Bold)).WordWrap())))))
                    .Content(dc => dc.Name("DocumentDataBand").CanGrow().Height(5).Elements(e => e
                        .Table(t => t.Layout(l => l.Left(0).Top(0).Height(5))
                            .Grid(
                                rows => rows
                                    .Row(r => r.Height(5)),
                                columns => columns
                                    .Column(c => c.Width(40))
                                    .Column(c => c.Width(40))
                                    .Column(c => c.Width(40)),
                                cells => cells
                                    .Cell(0, 0, c => c
                                        .Bind(b => b.Property("Document", "Patient.LastName"))
                                        .Border(b => b.All(l => l.Color("Black").Width(1).Style(BorderLineStyle.Solid)))
                                        .Style(s => s.Foreground("Black").Background("White").HorizontalAlignment(HorizontalAlignment.Left).VerticalAlignment(VerticalAlignment.Center).Font("Arial", 7).WordWrap()))
                                    .Cell(0, 1, c => c
                                        .Bind(b => b.Property("Document", "Patient.FirstName"))
                                        .Border(b => b.All(l => l.Color("Black").Width(1).Style(BorderLineStyle.Solid)))
                                        .Style(s => s.Foreground("Black").Background("White").HorizontalAlignment(HorizontalAlignment.Left).VerticalAlignment(VerticalAlignment.Center).Font("Arial", 7).WordWrap()))
                                    .Cell(0, 2, c => c
                                        .Bind(b => b.Property("Document", "Patient.MiddleName"))
                                        .Border(b => b.All(l => l.Color("Black").Width(1).Style(BorderLineStyle.Solid)))
                                        .Style(s => s.Foreground("Black").Background("White").HorizontalAlignment(HorizontalAlignment.Left).VerticalAlignment(VerticalAlignment.Center).Font("Arial", 7).WordWrap())))))))
                .BuildTemplate();

            var reportFactory = new FrReportBuilder();
            var report = (FRReport)reportFactory.Build(template).Object;
            report.Save(@"1.frx");

            var reportSerializer = new ReportTemplateSerializer();
            File.WriteAllBytes("1.json", reportSerializer.Serialize(template));

            ReportTemplate template2;

            using (var data = File.OpenRead("1.json"))
            {
                template2 = reportSerializer.Deserialize(data);
            }

            var report2 = (FRReport)reportFactory.Build(template2).Object;
            report2.Save(@"2.frx");
        }

        // ПАРАМЕТРЫ

        [Test]
        public void ShouldCreateReportWithParameters()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .Parameters(parameters => parameters
                    .Register("StringParameter", SchemaDataType.String)
                    .Register("IntParameter", SchemaDataType.Integer)
                    .Register("DateTimeParameter", SchemaDataType.DateTime))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Parameters.Count);
            Assert.AreEqual("StringParameter", result.Parameters[0].Name);
            Assert.AreEqual(typeof(string), result.Parameters[0].DataType);
            Assert.AreEqual("IntParameter", result.Parameters[1].Name);
            Assert.AreEqual(typeof(int), result.Parameters[1].DataType);
            Assert.AreEqual("DateTimeParameter", result.Parameters[2].Name);
            Assert.AreEqual(typeof(DateTime), result.Parameters[2].DataType);
        }

        // ИСТОЧНИКИ ДАННЫХ

        [Test]
        public void ShouldCreateReportWithComplexDataSource()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .DataSources(ds => ds.Register<Order>())
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            Assert.IsNotNull(result);

            var orderDataSource = result.Dictionary.DataSources.FindByName("Order");
            Assert.IsNotNull(orderDataSource);

            var clientProperty = orderDataSource.Columns.FindByName("Client");
            Assert.IsNotNull(clientProperty);
            Assert.IsNotNull(clientProperty.Columns.FindByName("FirstName"));
            Assert.IsNotNull(clientProperty.Columns.FindByName("LastName"));
            Assert.IsNotNull(clientProperty.Columns.FindByName("Discount"));

            var managerProperty = orderDataSource.Columns.FindByName("Manager");
            Assert.IsNotNull(managerProperty);
            Assert.IsNotNull(managerProperty.Columns.FindByName("FirstName"));
            Assert.IsNotNull(managerProperty.Columns.FindByName("LastName"));

            var itemsProperty = orderDataSource.Columns.FindByName("Items");
            Assert.IsNotNull(itemsProperty);
            Assert.IsNotNull(itemsProperty.Columns.FindByName("Quantity"));

            var itemsProductProperty = itemsProperty.Columns.FindByName("Product");
            Assert.IsNotNull(itemsProductProperty.Columns.FindByName("Name"));
            Assert.IsNotNull(itemsProductProperty.Columns.FindByName("Price"));
        }

        [Test]
        public void ShouldCreateReportWithMultiplyDataSource()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .DataSources(ds => ds.Register<Client>("DataSource1"))
                .DataSources(ds => ds.Register<Product>("DataSource2"))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Dictionary.DataSources.Count);
            Assert.IsNotNull(result.Dictionary.FindByName("DataSource1"));
            Assert.IsNotNull(result.Dictionary.FindByName("DataSource2"));
        }

        // НАСТРОЙКИ ПЕЧАТИ

        [Test]
        public void ShouldCreateReportWithPrintSetup()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .PrintSetup(print => print
                    .Paper(paper => paper.Width(10).Height(20).Landscape())
                    .Margin(margin => margin.Left(1).Right(2).Top(3).Bottom(4).MirrorOnEvenPages()))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);

            Assert.AreEqual(20, page.PaperWidth);
            Assert.AreEqual(10, page.PaperHeight);
            Assert.IsTrue(page.Landscape);

            Assert.AreEqual(1, page.LeftMargin);
            Assert.AreEqual(2, page.RightMargin);
            Assert.AreEqual(3, page.TopMargin);
            Assert.AreEqual(4, page.BottomMargin);
            Assert.IsTrue(page.MirrorMargins);
        }

        // ЗАГОЛОВОК ОТЧЕТА

        [Test]
        public void ShouldCreateReportWithTitle()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .Title(title => title.Height(10).CanGrow().CanShrink())
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then
            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ReportTitle);
            Assert.IsTrue(page.ReportTitle.CanGrow);
            Assert.IsTrue(page.ReportTitle.CanShrink);
            Assert.AreEqual(10 * Units.Millimeters, page.ReportTitle.Height);
        }

        [Test]
        public void ShouldSetBorderOfReportTitle()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .Title(title => title
                    .Border(border => border
                        .Left(line => line.Width(1).Color(ColorTranslator.ToHtml(Color.Red)).Style(BorderLineStyle.Dot))
                        .Right(line => line.Width(2).Color(ColorTranslator.ToHtml(Color.Green)).Style(BorderLineStyle.Dash))
                        .Top(line => line.Width(3).Color(ColorTranslator.ToHtml(Color.Blue)).Style(BorderLineStyle.DashDot))
                        .Bottom(line => line.Width(4).Color(ColorTranslator.ToHtml(Color.Yellow)).Style(BorderLineStyle.DashDotDot))))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);

            Assert.IsNotNull(page.ReportTitle);

            Assert.IsNotNull(page.ReportTitle.Border);
            Assert.IsNotNull(page.ReportTitle.Border.LeftLine);
            Assert.AreEqual(1, page.ReportTitle.Border.LeftLine.Width);
            Assert.AreEqual(Color.Red, page.ReportTitle.Border.LeftLine.Color);
            Assert.AreEqual(LineStyle.Dot, page.ReportTitle.Border.LeftLine.Style);

            Assert.IsNotNull(page.ReportTitle.Border.RightLine);
            Assert.AreEqual(2, page.ReportTitle.Border.RightLine.Width);
            Assert.AreEqual(Color.Green, page.ReportTitle.Border.RightLine.Color);
            Assert.AreEqual(LineStyle.Dash, page.ReportTitle.Border.RightLine.Style);

            Assert.IsNotNull(page.ReportTitle.Border.TopLine);
            Assert.AreEqual(3, page.ReportTitle.Border.TopLine.Width);
            Assert.AreEqual(Color.Blue, page.ReportTitle.Border.TopLine.Color);
            Assert.AreEqual(LineStyle.DashDot, page.ReportTitle.Border.TopLine.Style);

            Assert.IsNotNull(page.ReportTitle.Border.BottomLine);
            Assert.AreEqual(4, page.ReportTitle.Border.BottomLine.Width);
            Assert.AreEqual(Color.Yellow, page.ReportTitle.Border.BottomLine.Color);
            Assert.AreEqual(LineStyle.DashDotDot, page.ReportTitle.Border.BottomLine.Style);
        }

        [Test]
        public void ShouldSetReportBandPrintSetup()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .Title(title => title
                    .PrintSetup(print => print.PrintOn(PrintTargets.FirstPage | PrintTargets.LastPage)
                    .StartNewPage()))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);

            Assert.IsNotNull(page.ReportTitle);
            Assert.AreEqual(PrintOn.FirstPage | PrintOn.LastPage, page.ReportTitle.PrintOn);
            Assert.IsTrue(page.ReportTitle.StartNewPage);
        }

        // ИТОГИ ОТЧЕТА

        [Test]
        public void ShouldCreateReportWithSummary()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .Summary(summary => summary.Height(10).CanGrow().CanShrink())
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);

            Assert.IsNotNull(page.ReportSummary);
            Assert.IsTrue(page.ReportSummary.CanGrow);
            Assert.IsTrue(page.ReportSummary.CanShrink);
            Assert.AreEqual(10 * Units.Millimeters, page.ReportSummary.Height);
        }

        // ТЕКСТОВОЕ ПОЛЕ

        [Test]
        public void ShouldCreateTextElement()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .Title(title => title
                    .Height(1)
                    .Elements(elements => elements
                        .Text(text => text.CanGrow().CanShrink())))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ReportTitle);

            var textObject = page.ReportTitle.FindChildElement<TextObject>();
            Assert.IsNotNull(textObject);
            Assert.IsTrue(textObject.CanGrow);
            Assert.IsTrue(textObject.CanShrink);
        }

        [Test]
        public void ShouldCreateComplexTextElement()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .PrintSetup(print => print
                    .Paper(paper => paper.A4()))
                .Title(title => title.Height(20)
                    .Elements(elements => elements
                        .Text(text => text.CanGrow().CanShrink()
                            .Border(border => border
                                .All(line => line
                                    .Width(2)
                                    .Color(ColorTranslator.ToHtml(Color.Magenta))
                                    .Style(BorderLineStyle.Dot)))
                            .Layout(layout => layout
                                .Left(1f).Top(2f)
                                .Width(50).Height(10))
                            .Style(style => style
                                .Angle(10)
                                .Foreground(ColorTranslator.ToHtml(Color.Red))
                                .Background(ColorTranslator.ToHtml(Color.Gray))
                                .HorizontalAlignment(HorizontalAlignment.Center)
                                .VerticalAlignment(VerticalAlignment.Center)
                                .Font("Courier New", 14)
                                .WordWrap())
                            .Bind(bind => bind
                                .Constant("Заголовок отчета")))))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ReportTitle);

            var textObject = page.ReportTitle.FindChildElement<TextObject>();
            Assert.IsNotNull(textObject);
            Assert.IsTrue(textObject.CanGrow);
            Assert.IsTrue(textObject.CanShrink);

            // Границы текстового поля
            Assert.IsNotNull(textObject.Border);
            Assert.IsNotNull(textObject.Border.LeftLine);
            Assert.AreEqual(2, textObject.Border.LeftLine.Width);
            Assert.AreEqual(Color.Magenta, textObject.Border.LeftLine.Color);
            Assert.AreEqual(LineStyle.Dot, textObject.Border.LeftLine.Style);
            Assert.IsNotNull(textObject.Border.RightLine);
            Assert.AreEqual(2, textObject.Border.RightLine.Width);
            Assert.AreEqual(Color.Magenta, textObject.Border.RightLine.Color);
            Assert.AreEqual(LineStyle.Dot, textObject.Border.RightLine.Style);
            Assert.IsNotNull(textObject.Border.TopLine);
            Assert.AreEqual(2, textObject.Border.TopLine.Width);
            Assert.AreEqual(Color.Magenta, textObject.Border.TopLine.Color);
            Assert.AreEqual(LineStyle.Dot, textObject.Border.TopLine.Style);
            Assert.IsNotNull(textObject.Border.BottomLine);
            Assert.AreEqual(2, textObject.Border.BottomLine.Width);
            Assert.AreEqual(Color.Magenta, textObject.Border.BottomLine.Color);
            Assert.AreEqual(LineStyle.Dot, textObject.Border.BottomLine.Style);

            // Расположение элемента
            Assert.AreEqual(1f * Units.Millimeters, textObject.Left, 0.001);
            Assert.AreEqual(2f * Units.Millimeters, textObject.Top, 0.001);
            Assert.AreEqual(50f * Units.Millimeters, textObject.Width, 0.001);
            Assert.AreEqual(10f * Units.Millimeters, textObject.Height, 0.001);

            // Начертание текста
            Assert.IsNotNull(textObject.Font);
            Assert.AreEqual("Courier New", textObject.Font.Name);
            Assert.AreEqual(14, textObject.Font.Size);
            Assert.AreEqual(10, textObject.Angle);
            Assert.AreEqual(Color.Red, textObject.TextColor);
            Assert.AreEqual(Color.Gray, textObject.FillColor);
            Assert.AreEqual(HorzAlign.Center, textObject.HorzAlign);
            Assert.AreEqual(VertAlign.Center, textObject.VertAlign);
            Assert.IsTrue(textObject.WordWrap);

            // Текст
            Assert.AreEqual("Заголовок отчета", textObject.Text);
        }

        [Test]
        public void ShouldCreateTextWithFormat()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .Parameters(parameters => parameters
                    .Register("DateFrom", SchemaDataType.DateTime))
                .Title(title => title
                    .Elements(elements => elements
                        .Text(text => text
                            .Bind(bind => bind.Parameter("DateFrom"))
                            .Format(format => format.Date().Long()))))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ReportTitle);

            var textObject = page.ReportTitle.FindChildElement<TextObject>();
            Assert.IsNotNull(textObject);

            Assert.IsInstanceOf<global::FastReport.Format.DateFormat>(textObject.Format);
            Assert.AreEqual("D", ((global::FastReport.Format.DateFormat)textObject.Format).Format);
        }

        // ПРИВЯЗКА ДАННЫХ

        [Test]
        public void ShouldCreateTextElementWithConstantBind()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .Title(title => title
                    .Elements(elements => elements
                        .Text(text => text
                            .Bind(bind => bind
                                .Constant("Constant")))))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ReportTitle);

            var textObject = page.ReportTitle.FindChildElement<TextObject>();
            Assert.IsNotNull(textObject);
            Assert.AreEqual("Constant", textObject.Text);
        }

        [Test]
        public void ShouldCreateTextElementWithTotalBind()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .DataSources(sources => sources
                    .Register<OrderItem>())
                .Totals("DataBand1", t => t
                    .Sum("TotalQuantity", "ReportSummaryBand1", e => e.Property("OrderItem", "Quantity")))
                .Data("OrderItem", data => data
                    .Content(c => c
                        .Name("DataBand1")))
                .Summary(summary => summary
                    .Name("ReportSummaryBand1")
                    .Elements(elements => elements
                        .Text(text => text
                            .Bind(bind => bind
                                .Total("TotalQuantity")))))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ReportSummary);

            var textObject = page.ReportSummary.FindChildElement<TextObject>();
            Assert.IsNotNull(textObject);
            Assert.AreEqual("[TotalQuantity]", textObject.Text);
        }

        [Test]
        public void ShouldCreateTextElementWithParameterBind()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .Parameters(parameter => parameter
                    .Register("Parameter1", SchemaDataType.String))
                .Title(title => title
                    .Elements(elements => elements
                        .Text(text => text
                            .Bind(bind => bind
                                .Parameter("Parameter1")))))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ReportTitle);

            var textObject = page.ReportTitle.FindChildElement<TextObject>();
            Assert.IsNotNull(textObject);
            Assert.AreEqual("[Parameter1]", textObject.Text);
        }

        [Test]
        public void ShouldCreateTextElementWithDataSourcePropertyBind()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .DataSources(data => data
                    .Register<Product>("DataSource1"))
                .Title(title => title
                    .Elements(elements => elements
                        .Text(text => text
                            .Bind(bind => bind.Property("DataSource1", "Name")))))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ReportTitle);

            var textObject = page.ReportTitle.FindChildElement<TextObject>();
            Assert.IsNotNull(textObject);
            Assert.AreEqual("[DataSource1.Name]", textObject.Text);
        }

        [Test]
        public void ShouldCreateReportWithDataBandAndBindData()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .DataSources(sources => sources
                    .Register<Product>())
                .Data("Product", "", data => { })
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);

            var dataBand = page.FindChildElement<DataBand>();
            Assert.IsNotNull(dataBand);
            Assert.IsNotNull(dataBand.DataSource);
            Assert.AreEqual("Product", dataBand.DataSource.Name);
        }

        [Test]
        public void ShouldCreateReportWithDataBandAndBindComplexData()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .DataSources(sources => sources
                    .Register<Order>())
                .Data("Order", "Items.$", data => { })
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);

            var dataBand = page.FindChildElement<DataBand>();
            Assert.IsNotNull(dataBand);
            Assert.IsNotNull(dataBand.DataSource);
            Assert.AreEqual("Items", dataBand.DataSource.Name);
        }

        // СТРАНИЦЫ ОТЧЕТА

        [Test]
        public void ShouldCreateReportWithPageHeaderAndFooter()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .Page(p => p
                    .Header(band => band.CanGrow())
                    .Footer(band => band.CanGrow()))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);

            var pageHeader = page.PageHeader;
            Assert.IsNotNull(pageHeader);
            Assert.IsTrue(pageHeader.CanGrow);
            Assert.IsFalse(pageHeader.CanShrink);

            var pageFooter = page.PageFooter;
            Assert.IsNotNull(pageFooter);
            Assert.IsTrue(pageFooter.CanGrow);
            Assert.IsFalse(pageFooter.CanShrink);
        }

        // ГРУППЫ ОТЧЕТА

        [Test]
        public void ShouldCreateReportWithGroups()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .DataSources(sources => sources
                    .Register<Order>())
                .Data("Order", data => data
                    .Group("Client.FirstName", group => group
                        .Footer(band => { })
                        .Header(band => { })))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(result);
            Assert.IsNotNull(page);

            var groupHeader = page.FindChildElement<GroupHeaderBand>();
            Assert.IsNotNull(groupHeader);
            Assert.AreEqual("[Order.Client.FirstName]", groupHeader.Condition);

            Assert.IsNotNull(groupHeader.Data);
            Assert.AreEqual("Order", groupHeader.Data.DataSource.Name);

            var groupFooter = groupHeader.FindChildElement<GroupFooterBand>();
            Assert.IsNotNull(groupFooter);
        }

        // ИТОГИ ОТЧЕТА

        [Test]
        public void ShouldCreateReportTemplateWithTotals1()
        {
            // Given / When

            var template = CreateReportTemplateConfig()
                .DataSources(sources => sources
                    .Register<Product>("DataSource1"))
                .Totals("DataBand1", totals => totals
                    .Min("MinPrice", "ReportSummaryBand1", e => e.Property("DataSource1", "Price"))
                    .Max("MaxPrice", "ReportSummaryBand1", e => e.Property("DataSource1", "Price"))
                    .Avg("AvgPrice", "ReportSummaryBand1", e => e.Property("DataSource1", "Price"))
                    .Sum("SumPrice", "ReportSummaryBand1", e => e.Property("DataSource1", "Price"))
                    .Count("CountPrice", "ReportSummaryBand1"))
                .Data("DataSource1", data => data
                    .Content(c => c
                        .Name("DataBand1")))
                .Summary(summary => summary
                    .Name("ReportSummaryBand1"))
                .BuildTemplate();

            // Then

            Assert.IsNotNull(template);
            Assert.IsNotNull(template.Totals);

            var minPriceTotal = template.Totals.Resolve("MinPrice");
            Assert.IsNotNull(minPriceTotal);
            Assert.AreEqual("MinPrice", minPriceTotal.Name);
            Assert.AreEqual("DataBand1", minPriceTotal.DataBand);
            Assert.AreEqual("ReportSummaryBand1", minPriceTotal.PrintBand);
            Assert.AreEqual(TotalFunc.Min, minPriceTotal.TotalFunc);
            Assert.IsInstanceOf<PropertyBind>(minPriceTotal.Expression);
            Assert.AreEqual("DataSource1", ((PropertyBind)minPriceTotal.Expression).DataSource);
            Assert.AreEqual("Price", ((PropertyBind)minPriceTotal.Expression).Property);

            var maxPriceTotal = template.Totals.Resolve("MaxPrice");
            Assert.IsNotNull(maxPriceTotal);
            Assert.AreEqual("MaxPrice", maxPriceTotal.Name);
            Assert.AreEqual("DataBand1", maxPriceTotal.DataBand);
            Assert.AreEqual("ReportSummaryBand1", maxPriceTotal.PrintBand);
            Assert.AreEqual(TotalFunc.Max, maxPriceTotal.TotalFunc);
            Assert.IsInstanceOf<PropertyBind>(maxPriceTotal.Expression);
            Assert.AreEqual("DataSource1", ((PropertyBind)maxPriceTotal.Expression).DataSource);
            Assert.AreEqual("Price", ((PropertyBind)maxPriceTotal.Expression).Property);

            var avgPriceTotal = template.Totals.Resolve("AvgPrice");
            Assert.IsNotNull(avgPriceTotal);
            Assert.AreEqual("AvgPrice", avgPriceTotal.Name);
            Assert.AreEqual("DataBand1", avgPriceTotal.DataBand);
            Assert.AreEqual("ReportSummaryBand1", avgPriceTotal.PrintBand);
            Assert.AreEqual(TotalFunc.Avg, avgPriceTotal.TotalFunc);
            Assert.IsInstanceOf<PropertyBind>(avgPriceTotal.Expression);
            Assert.AreEqual("DataSource1", ((PropertyBind)avgPriceTotal.Expression).DataSource);
            Assert.AreEqual("Price", ((PropertyBind)avgPriceTotal.Expression).Property);

            var sumPriceTotal = template.Totals.Resolve("SumPrice");
            Assert.IsNotNull(sumPriceTotal);
            Assert.AreEqual("SumPrice", sumPriceTotal.Name);
            Assert.AreEqual("DataBand1", sumPriceTotal.DataBand);
            Assert.AreEqual("ReportSummaryBand1", sumPriceTotal.PrintBand);
            Assert.AreEqual(TotalFunc.Sum, sumPriceTotal.TotalFunc);
            Assert.IsInstanceOf<PropertyBind>(sumPriceTotal.Expression);
            Assert.AreEqual("DataSource1", ((PropertyBind)sumPriceTotal.Expression).DataSource);
            Assert.AreEqual("Price", ((PropertyBind)sumPriceTotal.Expression).Property);

            var countProductsTotal = template.Totals.Resolve("CountPrice");
            Assert.IsNotNull(countProductsTotal);
            Assert.AreEqual("CountPrice", countProductsTotal.Name);
            Assert.AreEqual("DataBand1", countProductsTotal.DataBand);
            Assert.AreEqual("ReportSummaryBand1", countProductsTotal.PrintBand);
            Assert.AreEqual(TotalFunc.Count, countProductsTotal.TotalFunc);
        }

        // ИТОГИ ГРУППЫ

        [Test]
        public void ShouldCreateReportTemplateWithTotals3()
        {
            // Given / When

            var template = CreateReportTemplateConfig()
                .DataSources(sources => sources
                    .Register<Order>())
                .Totals("OrderDataBand", totals => totals
                    .Max("MaxDiscountOfReport", "ReportSummary", e => e.Property("Order", "Client.Discount"))
                    .Max("MaxDiscountOfGroup", "OrderGroup", e => e.Property("Order", "Client.Discount"))
                    .Max("MaxDiscountOfSubgroup", "OrderSubgroup", e => e.Property("Order", "Client.Discount")))
                .Data("Order", "", data => data.Content(c => c.Name("OrderDataBand"))
                    .Group("Manager.FirstName", group => group.Footer(c => c.Name("OrderGroup")))
                    .Group("Client.FirstName", subgroup => subgroup.Footer(c => c.Name("OrderSubgroup"))))
                .Summary(c => c.Name("ReportSummary"))
                .BuildTemplate();

            // Then

            Assert.IsNotNull(template);
            Assert.IsNotNull(template.Totals);

            var maxDiscountOfReport = template.Totals.Resolve("MaxDiscountOfReport");
            Assert.IsNotNull(maxDiscountOfReport);
            Assert.AreEqual("OrderDataBand", maxDiscountOfReport.DataBand);
            Assert.AreEqual("ReportSummary", maxDiscountOfReport.PrintBand);
            Assert.AreEqual(TotalFunc.Max, maxDiscountOfReport.TotalFunc);
            Assert.IsInstanceOf<PropertyBind>(maxDiscountOfReport.Expression);
            Assert.AreEqual("Order", ((PropertyBind)maxDiscountOfReport.Expression).DataSource);
            Assert.AreEqual("Client.Discount", ((PropertyBind)maxDiscountOfReport.Expression).Property);

            var maxDiscountOfGroup = template.Totals.Resolve("MaxDiscountOfGroup");
            Assert.IsNotNull(maxDiscountOfGroup);
            Assert.AreEqual("OrderDataBand", maxDiscountOfGroup.DataBand);
            Assert.AreEqual("OrderGroup", maxDiscountOfGroup.PrintBand);
            Assert.AreEqual(TotalFunc.Max, maxDiscountOfGroup.TotalFunc);
            Assert.IsInstanceOf<PropertyBind>(maxDiscountOfGroup.Expression);
            Assert.AreEqual("Order", ((PropertyBind)maxDiscountOfGroup.Expression).DataSource);
            Assert.AreEqual("Client.Discount", ((PropertyBind)maxDiscountOfGroup.Expression).Property);

            var maxDiscountOfSubgroup = template.Totals.Resolve("MaxDiscountOfSubgroup");
            Assert.IsNotNull(maxDiscountOfSubgroup);
            Assert.AreEqual("OrderDataBand", maxDiscountOfSubgroup.DataBand);
            Assert.AreEqual("OrderSubgroup", maxDiscountOfSubgroup.PrintBand);
            Assert.AreEqual(TotalFunc.Max, maxDiscountOfSubgroup.TotalFunc);
            Assert.IsInstanceOf<PropertyBind>(maxDiscountOfSubgroup.Expression);
            Assert.AreEqual("Order", ((PropertyBind)maxDiscountOfSubgroup.Expression).DataSource);
            Assert.AreEqual("Client.Discount", ((PropertyBind)maxDiscountOfSubgroup.Expression).Property);
        }

        [Test]
        public void ShouldCreateReportWithTable()
        {
            // Given
            var template = CreateReportTemplateConfig()
                .PrintSetup(print => print.Paper(p => p.A4()))
                .Title(title => title.Height(100)
                    .Elements(elements => elements
                        .Table(table => table
                            .Grid(
                                rows => rows
                                    .Row(r => r.Height(5))
                                    .Row(r => r.Height(15))
                                    .Row(r => r.Height(5)),
                                columns => columns
                                    .Column(c => c.Width(20))
                                    .Column(c => c.Width(20))
                                    .Column(c => c.Width(20)),
                                cells => cells
                                    .Cell(0, 0, c => c.Bind(b => b.Constant("карьер")).ColSpan(3))
                                    .Cell(1, 0, c => c.Bind(b => b.Constant("время входа")))
                                    .Cell(1, 1, c => c.Bind(b => b.Constant("время выхода")))
                                    .Cell(1, 2, c => c.Bind(b => b.Constant("время нахождения")))
                                    .Cell(2, 0, c => c.Bind(b => b.Constant("час")))
                                    .Cell(2, 1, c => c.Bind(b => b.Constant("час")))
                                    .Cell(2, 2, c => c.Bind(b => b.Constant("мин")))))))
                .BuildTemplate();

            // When
            var result = template.BuildFastReport();

            // Then

            var page = result.FirstPage();
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ReportTitle);

            var tableElement = page.ReportTitle.FindChildElement<TableObject>();
            Assert.IsNotNull(tableElement);
            Assert.AreEqual(3, tableElement.Rows.Count);
            Assert.AreEqual(3, tableElement.Columns.Count);

            var tableRow = tableElement.FindChildElement<TableRow>();
            Assert.IsNotNull(tableRow);
            Assert.AreEqual(5 * Units.Millimeters, tableRow.Height);
            Assert.IsFalse(tableRow.AutoSize);

            var tableColumn = tableElement.FindChildElement<TableColumn>();
            Assert.IsNotNull(tableColumn);
            Assert.AreEqual(20 * Units.Millimeters, tableColumn.Width);
            Assert.IsFalse(tableColumn.AutoSize);

            var tableCell = tableElement[0, 0];
            Assert.IsNotNull(tableCell);
            Assert.AreEqual("карьер", tableCell.Text);
            Assert.AreEqual(1, tableCell.RowSpan);
            Assert.AreEqual(3, tableCell.ColSpan);
        }


        private static ReportTemplateConfig CreateReportTemplateConfig()
        {
            return ReportTemplateFluent.Report("TestReport");
        }
    }
}