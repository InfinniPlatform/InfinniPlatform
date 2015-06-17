﻿using System.Linq;

namespace InfinniPlatform.Utils
{
	class Program
	{
		static void Main(string[] args)
		{
			var command = args[0];
			var arguments = args.Skip(1).ToArray();

			switch (command)
			{
				case "upload":
					new ConfigManager().Upload(arguments.Length > 0 ? arguments[0] : null, arguments.Length > 1);
					break;
				case "download":
					new ConfigManager().Download(arguments.Length > 0 ? arguments[0] : null, arguments.Length > 1 ? arguments[1] : null);
					break;
				case "importdata":
					new DataManager().Import(arguments.Length > 0 ? arguments[0] : null);
					break;
			}
		}
	}
}