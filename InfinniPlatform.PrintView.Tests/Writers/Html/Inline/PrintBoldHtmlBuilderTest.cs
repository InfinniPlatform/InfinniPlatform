﻿using System.Collections.Generic;

using InfinniPlatform.PrintView.Inline;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintBoldHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildBold()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildBold)}.txt");

            var element = new PrintBold
                          {
                              Inlines = new List<PrintInline>
                                        {
                                            new PrintRun { Text = "Bold Text." }
                                        }
                          };

            // When
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var result = new TextWriterWrapper();
            context.Build(element, result.Writer);

            // Then
            Assert.AreEqual(expectedResult, result.GetText());
        }
    }
}