using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Compression;
using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Compression
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ZipArchiveTest
	{
		/// <summary>
		/// Copies the contents of input to output. Doesn't close either stream.
		/// </summary>
		public static void CopyStream(Stream input, Stream output)
		{
			byte[] buffer = new byte[8 * 1024];
			int len;
			while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
			{
				output.Write(buffer, 0, len);
			}
		}


		[Test]
		public void ShouldLoadZipArchive()
		{
			//given
			var pathAssemblies = Path.Combine("TestData", "Assemblies.zip");
			var streamArchive = new FileStream(pathAssemblies, FileMode.Open);
			
			//when
			var zipArchive = streamArchive.ReadArchive(Encoding.Default);

			//then
			string filename = "InfinniConfiguration.Integration.dll";
			zipArchive.UnzipFile(filename,stream =>
				                                                            {
																				using (Stream file = File.Create(filename))
																				{
																					CopyStream(stream, file);
																				}
					                                                            var bytes = File.ReadAllBytes(filename);
																				Assert.True(bytes.Count() > 0);
				                                                            });
		}
	}
}
