using InfinniPlatform.Api.SelfDocumentation;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Help
{
	[TestFixture]
	[Category(TestCategories.BusinessLogicTest)]
	public sealed class SelfDocumentationBehavior
	{
		[Test]
        [Ignore("Починить тест документации")]
		public void ShouldGenerateHelp()
		{
			var docs = DocumentationKeeperBuilder.Build(
				"InfinniConfiguration.Classifiers.Tests.dll",
				new HtmlDocumentationFormatter());

			docs.SaveHelp("ClassifierStorage");
			docs.SaveHelp("ClassifierLoader");

			Assert.IsFalse(string.IsNullOrEmpty(DocumentationKeeper.ReadHelpFromFile("ClassifierStorage")));
			Assert.IsFalse(string.IsNullOrEmpty(DocumentationKeeper.ReadHelpFromFile("ClassifierLoader")));
		}
	}
}