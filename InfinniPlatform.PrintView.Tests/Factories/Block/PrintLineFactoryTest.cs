using InfinniPlatform.PrintView.Abstractions.Block;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Block
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintLineFactoryTest
    {
        [Test]
        public void ShouldBuild()
        {
            // Given
            var template = new PrintLine();

            // When
            var element = BuildTestHelper.BuildElement<PrintLine>(template);

            // Then
            Assert.IsNotNull(element);
        }
    }
}