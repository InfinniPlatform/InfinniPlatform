using InfinniPlatform.PrintView.Abstractions.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintLineBreakFactoryTest
    {
        [Test]
        public void ShouldBuildLineBreak()
        {
            // Given
            var template = new PrintLineBreak();

            // When
            var element = BuildTestHelper.BuildElement<PrintLineBreak>(template);

            // Then
            Assert.IsNotNull(element);
        }
    }
}