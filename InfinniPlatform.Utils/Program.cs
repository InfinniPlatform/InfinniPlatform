using System;
using System.Linq;

namespace InfinniPlatform.Utils
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var command = args[0];
            var arguments = args.Skip(1).ToArray();

            switch (command)
            {
                case "upload":
                    new ConfigManager().Upload(arguments.Length > 0 ? arguments[0] : string.Empty, arguments.Length > 0);
                    break;
                case "download":
                    if (arguments.Length > 3)
                    {
                        new ConfigManager().Download(arguments[0], arguments[1], arguments[2], arguments[3]);
                    }
                    else
                    {
                        Console.WriteLine("Solution export directory, solution name, solution version should specified.");
                    }
                    break;                    
                case "importdata":
                    new DataManager().Import(arguments.Length > 0 ? arguments[0] : null);
                    break;
            }
        }
    }
}