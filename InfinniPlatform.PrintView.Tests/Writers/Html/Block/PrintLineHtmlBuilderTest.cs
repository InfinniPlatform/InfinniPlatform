﻿using InfinniPlatform.PrintView.Block;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Writers.Html.Block
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintLineHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildLine()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildLine)}.txt");

            var element = new PrintLine();

            // When
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var result = new TextWriterWrapper();
            context.Build(element, result.Writer);

            // Then
            Assert.AreEqual(expectedResult, result.GetText());
        }
    }
}