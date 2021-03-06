﻿using System.Collections.Generic;

using InfinniPlatform.PrintView.Inline;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintUnderlineHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildUnderline()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildUnderline)}.txt");

            var element = new PrintUnderline
                          {
                              Inlines = new List<PrintInline>
                                        {
                                            new PrintRun { Text = "Underline Text." }
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