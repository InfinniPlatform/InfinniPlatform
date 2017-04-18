using InfinniPlatform.PrintView.Abstractions.Block;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Block
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