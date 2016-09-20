using System.Linq;

using InfinniPlatform.PrintView.Model.Inlines;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementItalicFactoryTest
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
            PrintElementItalic element = BuildTestHelper.BuildItalic(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Inlines);
            Assert.AreEqual(2, element.Inlines.Count);
            Assert.IsInstanceOf<PrintElementRun>(element.Inlines.First());
            Assert.IsInstanceOf<PrintElementRun>(element.Inlines.Last());
            Assert.AreEqual("Inline1", ((PrintElementRun) element.Inlines.First()).Text);
            Assert.AreEqual("Inline2", ((PrintElementRun) element.Inlines.Last()).Text);
        }
    }
}