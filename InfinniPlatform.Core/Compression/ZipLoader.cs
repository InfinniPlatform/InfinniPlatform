using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Compression
{
	public static class ZipLoader
	{
		public static ZipArchive GetArchive(this Stream streamToUnzip, Encoding zipEncoding)
		{
			return new ZipArchive(streamToUnzip,ZipArchiveMode.Read,true,zipEncoding);			
		}

		public static IEnumerable<string> GetZipEntries(this ZipArchive zipArchive)
		{
			return zipArchive.Entries.Select(z => z.Name).ToList();
		} 

		public static void UnzipFile(this ZipArchive zipArchive, string zipEntry, Action<Stream> unzipAction)
		{
			var entry = zipArchive.GetEntry(zipEntry);
			if (entry == null)
			{
				throw new ArgumentException(string.Format("entry \"{0}\" not found in archive",zipEntry));
			}

			using (var stream = entry.Open())
			{
				if (unzipAction != null)
				{
					unzipAction(stream);
				}
			}
		}

	}
}
