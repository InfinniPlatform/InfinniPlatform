using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace InfinniPlatform.Core.Packages
{
    public static class ZipLoader
    {
        public static ZipArchive ReadArchive(this Stream streamToUnzip, Encoding zipEncoding)
        {
            return new ZipArchive(streamToUnzip, ZipArchiveMode.Read, true, zipEncoding);
        }

        public static ZipArchive ReadArchive(string filePath, FileStream fileStream, Encoding zipEncoding)
        {
            try
            {
                var zipArchive = ReadArchive(fileStream, zipEncoding);
                return zipArchive;
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    string.Format("Error on reading json configuration file archive: {0}, Error: {1}",
                        filePath, e.Message));
            }
        }

        public static void UnzipFile(this ZipArchive zipArchive, string zipEntry, Action<Stream> unzipAction)
        {
			var zipEntryAlternate =  zipEntry.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            var entry = zipArchive.GetEntry(zipEntry) ?? zipArchive.GetEntry(zipEntryAlternate);
            if (entry == null)
            {
                throw new ArgumentException(string.Format("entry \"{0}\" not found in archive", zipEntry));
            }

            using (var stream = entry.Open())
            {
                if (unzipAction != null)
                {
                    unzipAction(stream);
                }
            }
        }

        public static void AddFile(this ZipArchive zipArchive, string zipEntryName, IEnumerable<string> content)
        {
            var zipEntry = zipArchive.CreateEntry(zipEntryName);

            using (var writer = new StreamWriter(zipEntry.Open()))
            {
                foreach (var contentstring in content)
                {
                    writer.WriteLine(contentstring);
                }
            }
        }

        public static IEnumerable<ZipArchiveEntry> GetFolderEntries(this ZipArchive zipArchive, string folderName)
        {
            return zipArchive.Entries.Where(w => IsChildEntry(w.FullName, folderName, true)).ToList();
        }

        public static IEnumerable<ZipArchiveEntry> GetFileEntries(this ZipArchive zipArchive, string folderName)
        {
            return zipArchive.Entries.Where(w => IsChildEntry(w.FullName, folderName, false)).ToList();
        }

        private static bool IsChildEntry(string folderName, string folderParent, bool folders)
        {
            var folderNames = folderName.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            var folderNamesParent = folderParent.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            if (folderNames.Count() < folderNamesParent.Count())
            {
                return false;
            }

            for (var i = 0; i < folderNamesParent.Count(); i++)
            {
                if (folderNames[i].ToLowerInvariant() != folderNamesParent[i].ToLowerInvariant())
                {
                    return false;
                }
            }
            if (folderNames.Count() - folderNamesParent.Count() == 1)
            {
                if (folderNames[folderNames.Count() - 1].Contains("."))
                {
                    return !folders;
                }
                return folders;
            }
            return false;
        }
    }
}