using System.Reflection;
using System.Runtime.Loader;

namespace InfinniPlatform.Conventions
{
    public class ReflectionOnlyAssemblyLoadContext : AssemblyLoadContext
    {
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }
}