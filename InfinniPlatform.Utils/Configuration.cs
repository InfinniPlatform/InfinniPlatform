using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace InfinniPlatform.Utils
{
    internal class Configuration
    {
        public Configuration(string pathString)
        {
            PathString = pathString;
            var pathConfig = Path.GetFileName(pathString).Split(new[] {"_"}, StringSplitOptions.RemoveEmptyEntries);
            if (pathConfig.Count() == 2)
            {
                Name = pathConfig[0].Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                Version = pathConfig[1];
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