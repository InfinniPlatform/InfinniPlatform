using System.IO;

namespace InfinniPlatform.Watcher
{
    public static class Extensions
    {
        public static string ToPartPath(this string s, string sourcePath)
        {
            return s.Replace(sourcePath, string.Empty).TrimStart(Path.DirectorySeparatorChar);
        }
    }
}