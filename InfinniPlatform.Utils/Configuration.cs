using System;
using System.IO;
using System.Linq;

namespace InfinniPlatform.Utils
{
    internal class Configuration
    {
        public Configuration(string pathString)
        {
            PathString = pathString;
            var pathConfig = Path.GetFileName(pathString).Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries);
            if (pathConfig.Count() == 3)
            {
                Name = pathConfig[0];
                Version = pathConfig[2];
            }
            else
            {
                throw new ArgumentException(string.Format("Not an configuration folder: {0}", pathString));
            }
        }

        public string PathString { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}