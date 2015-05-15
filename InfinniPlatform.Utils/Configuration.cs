using System.Text.RegularExpressions;

namespace InfinniPlatform.Utils
{
	class Configuration
	{
		public Configuration(string path)
		{
			Path = path;
			Name = Regex.Match(path, @"([\w\.]+?)\.Configuration").Groups[1].Value;
		}

		public string Path { get; set; }
		public string Name { get; set; }
	}
}