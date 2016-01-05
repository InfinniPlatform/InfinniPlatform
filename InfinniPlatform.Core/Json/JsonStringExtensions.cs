using System.IO;
using System.Text;

namespace InfinniPlatform.Core.Json
{
    public static class JsonStringExtensions
    {
        public static Stream AsStream(this string target)
        {
            return new MemoryStream(Encoding.Default.GetBytes(target ?? string.Empty));
        }
    }
}