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
                case "importdata":
                    DataManager.Import(arguments.Length > 0 ? arguments[0] : null);
                    break;
            }
        }
    }
}