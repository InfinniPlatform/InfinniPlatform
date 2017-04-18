using System.Linq;

using InfinniPlatform.PrintView.Abstractions.Format;
using InfinniPlatform.PrintView.Abstractions.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintHyperlinkFactoryTest
    {
        [Test]
        public void ShouldBuildInlines()
        {
            // Given

            var inline1 = new PrintRun { Text = "Inline1" };
            var inline2 = new PrintRun { Text = "Inline2" };

            var template = new PrintHyperlink { Reference = "http://some.com" };
            template.Inlines.Add(inline1);
            template.Inlines.Add(inline2);

            // When
            var element = BuildTestHelper.BuildElement<PrintHyperlink>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Inlines);
            Assert.AreEqual(template.Reference, element.Reference);
            Assert.AreEqual(template.Inlines.Count, element.Inlines.Count);
            Assert.IsInstanceOf<PrintRun>(element.Inlines.First());
            Assert.IsInstanceOf<PrintRun>(element.Inlines.Last());
            Assert.AreEqual(inline1.Text, ((PrintRun)element.Inlines.First()).Text);
            Assert.AreEqual(inline2.Text, ((PrintRun)element.Inlines.Last()).Text);
        }

        [Test]
        public void ShouldApplyReferenceFromSource()
        {
            // Given
            const string dataSource = "http://some.com";
            var template = new PrintHyperlink { Source = "$" };

            // When
            var element = BuildTestHelper.BuildElement<PrintHyperlink>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource, element.Reference);
        }

        [Test]
        public void ShouldApplyReferenceFromSourceFormat()
        {
            // Given

            var dataSource = new { Id = 12345 };
            var sourceFormat = new ObjectFormat { Format = "http://some.com/{Id}" };
            var template = new PrintHyperlink { Source = "$", SourceFormat = sourceFormat };

            // When
            var element = BuildTestHelper.BuildElement<PrintHyperlink>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("http://some.com/12345", element.Reference);
        }
    }
}