using System.Linq;

using InfinniPlatform.PrintView.Model.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintUnderlineFactoryTest
    {
        [Test]
        public void ShouldBuildInlines()
        {
            // Given

            var inline1 = new PrintRun { Text = "Inline1" };
            var inline2 = new PrintRun { Text = "Inline2" };

            var template = new PrintUnderline();
            template.Inlines.Add(inline1);
            template.Inlines.Add(inline2);

            // When
            var element = BuildTestHelper.BuildElement<PrintUnderline>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Inlines);
            Assert.AreEqual(template.Inlines.Count, element.Inlines.Count);
            Assert.IsInstanceOf<PrintRun>(element.Inlines.First());
            Assert.IsInstanceOf<PrintRun>(element.Inlines.Last());
            Assert.AreEqual(inline1.Text, ((PrintRun)element.Inlines.First()).Text);
            Assert.AreEqual(inline2.Text, ((PrintRun)element.Inlines.Last()).Text);
        }
    }
}