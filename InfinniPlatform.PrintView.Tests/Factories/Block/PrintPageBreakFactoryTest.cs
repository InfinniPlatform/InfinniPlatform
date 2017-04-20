using InfinniPlatform.PrintView.Block;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Factories.Block
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintPageBreakFactoryTest
    {
        [Test]
        public void ShouldBuild()
        {
            // Given
            var template = new PrintPageBreak();

            // When
            var element = BuildTestHelper.BuildElement<PrintPageBreak>(template);

            // Then
            Assert.IsNotNull(element);
        }
    }
}