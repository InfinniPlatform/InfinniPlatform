using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Blocks;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Blocks
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementLineFactoryTest
    {
        [Test]
        public void ShouldBuild()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();

            // When
            PrintElementBlock element = BuildTestHelper.BuildLine(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsInstanceOf<PrintElementLine>(element);
        }
    }
}