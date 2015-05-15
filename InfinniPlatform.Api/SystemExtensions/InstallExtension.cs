using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.SystemExtensions
{
    public static class InstallExtension
    {
        public static void SaveToFile(this Stream uploadStream, string filePath)
        {
            var folderPath = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (Stream file = File.Create(filePath))
            {
                CopyStream(uploadStream, file);
            }
        }


        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
