using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Text;

namespace InfinniPlatform.ServiceHost.Services
{
    public class Service
    {
        public string Get()
        {
            var enumerateFiles = Directory.EnumerateFiles(".", "*MessageQueue.dll", SearchOption.AllDirectories);

            try
            {
                var loadFromAssemblyPath = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(enumerateFiles.First()));

                var types = loadFromAssemblyPath.GetTypes();

                var stringBuilder = new StringBuilder();
                foreach (var type in types.OrderBy(type => type.FullName))
                {
                    stringBuilder.AppendLine(type.FullName);
                }

                return stringBuilder.ToString();
            }
            catch (Exception e)
            {
                return "NoAssembly";
            }
        }
    }
}