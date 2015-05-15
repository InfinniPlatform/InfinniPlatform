using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Json
{
    public static class JsonStringExtensions
    {
        public static Stream AsStream(this string target)
        {
            return new MemoryStream(Encoding.Default.GetBytes(target ?? string.Empty));
        }
    }
}
