using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Blocks;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Blocks
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementPageBreakFactoryTest
    {
        [Test]
        public void ShouldBuild()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();

            // When
            PrintElementBlock element = BuildTestHelper.BuildPageBreak(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsInstanceOf<PrintElementPageBreak>(element);
        }
    }
}