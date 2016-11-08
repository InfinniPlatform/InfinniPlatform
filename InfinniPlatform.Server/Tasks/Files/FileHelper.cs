using System.IO;

using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Server.Tasks.Files
{
    public static class FileHelper
    {
        public static object WrapLogResponse(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var text = reader.ReadToEnd();

                if (string.IsNullOrEmpty(text))
                {
                    text = "Log is empty.";
                }

                return new ServiceResult<string>
                       {
                           Success = true,
                           Result = text
                       };
            }
        }
    }
}