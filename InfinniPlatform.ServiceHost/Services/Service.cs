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
            var path = Path.Combine("C:\\Projects\\InfinniAspNet\\Assemblies\\netstandard1.6", "InfinniPlatform.MessageQueue.dll");

            try
            {
                var loadFromAssemblyPath = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);

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