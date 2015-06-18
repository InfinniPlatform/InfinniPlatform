using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace InfinniPlatform.Utils
{
	class Configuration
	{
		public Configuration(string path)
		{
			Path = path;
		    var pathConfig = path.Split(new [] {"."}, StringSplitOptions.RemoveEmptyEntries);
		    if (pathConfig.Count() == 3)
		    {
		        Name = pathConfig[0];
		        Version = pathConfig[2];
		    }
		    else
		    {
		        throw new ArgumentException(string.Format("Not an configuration folder: {0}",path));
		    }
		}

		public string Path { get; set; }

		public string Name { get; set; }

        public string Version { get; set; }
	}
}