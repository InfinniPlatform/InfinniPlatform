﻿using System.Collections.Generic;

using InfinniPlatform.PrintView.Inline;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintSpanHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildSpan()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildSpan)}.txt");

            var element = new PrintSpan
                          {
                              Inlines = new List<PrintInline>
                                        {
                                            new PrintRun { Text = "Inline1. " },
                                            new PrintRun { Text = "Inline2. " }
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