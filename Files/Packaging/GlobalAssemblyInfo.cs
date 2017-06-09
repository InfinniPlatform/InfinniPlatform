using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("Infinnity Solutions")]
[assembly: AssemblyProduct("InfinniPlatform")]
[assembly: AssemblyCopyright("Copyright © Infinnity Solutions 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyConfiguration("")]

// TeamCity File Content Replacer: Add build number
// Look in: */Packaging/GlobalAssemblyInfo.cs
// Find what: ((AssemblyVersion|AssemblyFileVersion)\s*\(\s*@?\")(?<major>[0-9]+)\.(?<minor>[0-9]+)\.(?<patch>[0-9]+)\.(?<build>[0-9]+)(\"\s*\))
// Replace with: $1$3.$4.$5.\%build.number%$7

[assembly: AssemblyVersion("2.3.8.0")]
[assembly: AssemblyFileVersion("2.3.8.0")]

// TeamCity File Content Replacer: Add VCS hash
// Look in: */Packaging/GlobalAssemblyInfo.cs
// Find what: (AssemblyInformationalVersion\s*\(\s*@?\").*?(\"\s*\))
// Replace with: $1\%build.vcs.number%$2

[assembly: AssemblyInformationalVersion("")]