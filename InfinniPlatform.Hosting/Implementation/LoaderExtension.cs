using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.SystemExtensions;
using InfinniPlatform.Hosting.Implementation.Modules;
using InfinniPlatform.Modules;

namespace InfinniPlatform.Hosting.Implementation
{
    public static class LoaderExtension
    {
        public static IEnumerable<InstallerInfo> GetInstallers(this Assembly assembly)
        {
            var result = new List<InstallerInfo>();
            try
            {
                foreach (var assemblyType in assembly.GetTypes())
                {
                    if (typeof(MetadataConfigurationInstaller).IsAssignableFrom(assemblyType))
                    {
                        var info = new InstallerInfo();
                        info.ConfigurationName = Regex.Replace(assemblyType.Name, "Installer", "", RegexOptions.IgnoreCase);

                        info.AssemblyName = assembly.GetName().Name;
                        result.Add(info);
                    }
                }
            }
            catch
            {

            }
            return result;
        }

        public static IEnumerable<string> InstallFromAssembly(this AssemblyInfo assembly, PackageBuilder packageBuilder, string versionName)
        {
            var installers = assembly.Assembly.GetInstallers();
            var result = new List<string>();
            foreach (var installer in installers)
            {
                dynamic package = packageBuilder.BuildPackage(
                    installer.ConfigurationName,
                    versionName,
                    installer.AssemblyName + (assembly.IsExecutable ? ".exe" : ".dll"));

                UpdateApi.InstallPackages(new[] { package });

                result.Add(package.ConfigurationName.ToString());
            }
            return result;
        }
    }
}
