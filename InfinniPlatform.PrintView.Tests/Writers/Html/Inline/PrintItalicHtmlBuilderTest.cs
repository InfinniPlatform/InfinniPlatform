﻿using System.Collections.Generic;

using InfinniPlatform.PrintView.Inline;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintItalicHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildItalic()
        {
            // Given

            var expectedResult = ResourceHelper.GetEmbeddedResourceText($"Writers.Html.Resources.{nameof(ShouldBuildItalic)}.txt");

            var element = new PrintItalic
                          {
                              Inlines = new List<PrintInline>
                                        {
                                            new PrintRun { Text = "Italic Text." }
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