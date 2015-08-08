using System.IO;
using System.Linq;
using System.Text;

using InfinniPlatform.Api.Packages;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Zip
{
	[TestFixture]
	[Category(TestCategories.AcceptanceTest)]
	public sealed class ZipLoaderBehavior
	{
		private static readonly string ClassifierLoaderPath = Path.Combine("TestData", "Zip", "ClassifierLoader.zip");

		[Test]
		public void ShouldGetFolderEntries()
		{
			using (var stream = new FileStream(ClassifierLoaderPath, FileMode.Open))
			{
				var zipArchive = ZipLoader.ReadArchive(stream,Encoding.UTF8);
				var entries = zipArchive.GetFolderEntries("Documents");
				Assert.AreEqual(entries.Count(), 2);
			}
			
		}

		[Test]
		public void ShouldGetFolderEntriesParented()
		{
			using (var stream = new FileStream(ClassifierLoaderPath, FileMode.Open))
			{
				var zipArchive = ZipLoader.ReadArchive(stream, Encoding.UTF8);
				var entries = zipArchive.GetFolderEntries("Documents");
				Assert.AreEqual(entries.Count(), 2);
			}
		}

		[Test]
		public void ShouldGetFolderEntriesParentedFolders()
		{
			using (var stream = new FileStream(ClassifierLoaderPath, FileMode.Open))
			{
				var zipArchive = ZipLoader.ReadArchive(stream, Encoding.UTF8);
				var entries = zipArchive.GetFolderEntries("Documents/Classifiers");
				Assert.AreEqual(entries.Count(), 4);
			}
		}

		[Test]
		public void ShouldGetFolderEntriesParentedFiles()
		{
			using (var stream = new FileStream(ClassifierLoaderPath, FileMode.Open))
			{
				var zipArchive = ZipLoader.ReadArchive(stream, Encoding.UTF8);
				var entries = zipArchive.GetFileEntries("Documents/Classifiers");
				Assert.AreEqual(entries.Count(), 1);
			}

		}

		[Test]
		public void ShouldGetFolderEntriesParentedFiles1()
		{
			using (var stream = new FileStream(ClassifierLoaderPath, FileMode.Open))
			{
				var zipArchive = ZipLoader.ReadArchive(stream, Encoding.UTF8);
				var entries = zipArchive.GetFileEntries("Documents/Classifiers/Processes");
				Assert.AreEqual(entries.Count(), 2);
			}
		}
	}
}
