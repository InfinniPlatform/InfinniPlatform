using System;
using System.Windows.Documents;
using InfinniPlatform.Sdk.Application.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementHyperlinkFactoryTest
    {
        [Test]
        public void ShouldBuildInlines()
        {
            // Given

            dynamic inline1 = new DynamicWrapper();
            inline1.Run = new DynamicWrapper();
            inline1.Run.Text = "Inline1";

            dynamic inline2 = new DynamicWrapper();
            inline2.Run = new DynamicWrapper();
            inline2.Run.Text = "Inline2";

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Inlines = new[] {inline1, inline2};

            // When
            Hyperlink element = BuildTestHelper.BuildHyperlink(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Inlines);
            Assert.AreEqual(2, element.Inlines.Count);
            Assert.IsInstanceOf<Run>(element.Inlines.FirstInline);
            Assert.IsInstanceOf<Run>(element.Inlines.LastInline);
            Assert.AreEqual("Inline1", ((Run) element.Inlines.FirstInline).Text);
            Assert.AreEqual("Inline2", ((Run) element.Inlines.LastInline).Text);
        }

        [Test]
        public void ShouldApplyReference()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Reference = "http://some.com";

            // When
            Hyperlink element = BuildTestHelper.BuildHyperlink(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(new Uri("http://some.com"), element.NavigateUri);
        }

        [Test]
        public void ShouldApplyReferenceFromSource()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$";

            // When
            var element = BuildTestHelper.BuildHyperlink((object) elementMetadata,
                c => { c.PrintViewSource = "http://some.com"; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(new Uri("http://some.com"), element.NavigateUri);
        }

        [Test]
        public void ShouldApplyReferenceFromSourceFormat()
        {
            // Given

            dynamic sourceFormat = new DynamicWrapper();
            sourceFormat.ObjectFormat = new DynamicWrapper();
            sourceFormat.ObjectFormat.Format = "http://some.com/{Id}";

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$";
            elementMetadata.SourceFormat = sourceFormat;

            // When
            var element = BuildTestHelper.BuildHyperlink((object) elementMetadata,
                c => { c.PrintViewSource = new {Id = 12345}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(new Uri("http://some.com/12345"), element.NavigateUri);
        }
    }
}