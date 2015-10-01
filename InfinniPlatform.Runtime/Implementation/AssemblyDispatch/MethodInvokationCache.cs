using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfinniPlatform.Runtime.Implementation.AssemblyDispatch
{
	[Obsolete]
	internal sealed class MethodInvokationCache : IMethodInvokationCache
	{
		public MethodInvokationCache(string version, DateTime timeStamp, IEnumerable<Assembly> versionAssemblies)
		{
			_version = version;
			_timeStamp = timeStamp;
			_versionAssemblies = versionAssemblies.ToList();
		}


		private readonly string _version;
		private readonly DateTime _timeStamp;
		private volatile List<Assembly> _versionAssemblies;
		private readonly object _versionAssembliesLock = new object();


		public string Version
		{
			get { return _version; }
		}

		public DateTime TimeStamp
		{
			get { return _timeStamp; }
		}


		public void AddVersionAssembly(IEnumerable<Assembly> assemblies)
		{
			lock (_versionAssembliesLock)
			{
				_versionAssemblies = _versionAssemblies.Where(v => assemblies.All(asm => asm.FullName != v.FullName)).ToList();
				_versionAssemblies.AddRange(assemblies);
			}
		}

		public MethodInfo FindMethodInfo(string type, string method)
		{
			// Сложность данного алгоритма O(n^3), как минимум.
			// Ранее реализованный "кэш" не заработал, да и не должен был заработать, поскольку
			// был настоящий "DLL Hell", т.к. сборки без версий, а прикладные сборки таковыми
			// и являются, не могут быть загружены в один AppDomain. Они вынуждены конфликтовать.
			// Учитывая новую концепцию развертывания версий прикладных решений в кластере, весь
			// этот механизм придется модифицировать в ближайшее время. Однако из-за проблем с
			// производительностью была сделана альтернативыная реализация данного "кэша".

			var methodInfo = ReflectionExtensions.FindMethodInfo(_versionAssemblies, type, method);
			return methodInfo.FirstOrDefault();
		}
	}
}