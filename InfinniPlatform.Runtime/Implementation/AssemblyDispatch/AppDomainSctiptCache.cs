using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace InfinniPlatform.Runtime.Implementation.AssemblyDispatch
{
	/// <summary>
	/// Кэш скриптов текущего домена приложения <see cref="AppDomain.CurrentDomain"/>.
	/// </summary>
	internal sealed class AppDomainSctiptCache
	{
		public AppDomainSctiptCache()
		{
			_scripts = new Lazy<ConcurrentDictionary<string, Lazy<MethodInfo>>>(LoadScripts);
		}


		private readonly Lazy<ConcurrentDictionary<string, Lazy<MethodInfo>>> _scripts;


		/// <summary>
		/// Возвращает метод для вызова скрипта.
		/// </summary>
		/// <param name="type">Имя типа скрипта.</param>
		/// <param name="method">Имя метода скрипта.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <returns>Метод для вызова скрипта.</returns>
		/// <remarks>
		/// В текущей реализации имя типа <paramref name="type"/> должно указываться без Namespace,
		/// так в конфигурации ссылка на скрипт хранится именно в таком виде.
		/// </remarks>
		public Lazy<MethodInfo> GetScriptMethod(string type, string method)
		{
			if (string.IsNullOrEmpty(type))
			{
				throw new ArgumentNullException("type");
			}

			if (string.IsNullOrEmpty(method))
			{
				throw new ArgumentNullException("method");
			}

			var scriptKey = CombineKey(type, method);

			Lazy<MethodInfo> scriptMethod;

			_scripts.Value.TryGetValue(scriptKey, out scriptMethod);

			return scriptMethod;
		}


		private ConcurrentDictionary<string, Lazy<MethodInfo>> LoadScripts()
		{
			var result = new ConcurrentDictionary<string, Lazy<MethodInfo>>(StringComparer.Ordinal);

			var scripts = FindScriptsWithoutLoad();

			if (scripts != null)
			{
				foreach (var scriptInfo in scripts)
				{
					// В текущей конфигурации имя типа указывается без Namespace!

					var info = scriptInfo;
					var scriptKey = CombineKey(info.TypeName, info.MethodName);
					result[scriptKey] = new Lazy<MethodInfo>(() => GetOrLoadMethod(info.AssemblyPath, info.TypeFullName, info.MethodName));
				}
			}

			return result;
		}


		private readonly ConcurrentDictionary<string, MethodInfo> _scriptMethods
			= new ConcurrentDictionary<string, MethodInfo>(StringComparer.Ordinal);

		private MethodInfo GetOrLoadMethod(string assemblyPath, string typeFullName, string methodName)
		{
			MethodInfo method;

			var methodKey = CombineKey(CombineKey(assemblyPath, typeFullName), methodName);

			if (!_scriptMethods.TryGetValue(methodKey, out method))
			{
				var type = GetOrLoadType(assemblyPath, typeFullName);

				method = type.GetMethod(methodName);

				_scriptMethods.TryAdd(methodKey, method);
			}

			return method;
		}


		private readonly ConcurrentDictionary<string, Type> _scriptTypes
			= new ConcurrentDictionary<string, Type>(StringComparer.Ordinal);

		private Type GetOrLoadType(string assemblyPath, string typeFullName)
		{
			Type type;

			var typeKey = CombineKey(assemblyPath, typeFullName);

			if (!_scriptTypes.TryGetValue(typeKey, out type))
			{
				var assembly = GetOrLoadAssembly(assemblyPath);

				type = assembly.GetType(typeFullName);

				_scriptTypes.TryAdd(typeKey, type);
			}

			return type;
		}


		private readonly ConcurrentDictionary<string, Assembly> _scriptAssemblies
			= new ConcurrentDictionary<string, Assembly>(StringComparer.OrdinalIgnoreCase);

		private Assembly GetOrLoadAssembly(string assemblyPath)
		{
			Assembly assembly;

			if (!_scriptAssemblies.TryGetValue(assemblyPath, out assembly))
			{
				assembly = Assembly.LoadFrom(assemblyPath);

				_scriptAssemblies.TryAdd(assemblyPath, assembly);
			}

			return assembly;
		}


		private static IEnumerable<ScriptInfo> FindScriptsWithoutLoad()
		{
			// Поиск скриптов без загрузки сборок, в которых они находятся,
			// чтобы не "засорять" текущий домен приложения раньше времени.

			IEnumerable<ScriptInfo> result;

			var currentDomainInfo = AppDomain.CurrentDomain.SetupInformation;

			var tempDomain = AppDomain.CreateDomain("TempDomain", null, new AppDomainSetup
																		{
																			LoaderOptimization = LoaderOptimization.MultiDomainHost,
																			ShadowCopyFiles = currentDomainInfo.ShadowCopyFiles,
																			ApplicationBase = currentDomainInfo.ApplicationBase
																		});

			try
			{
				tempDomain.DoCallBack(() =>
									  {
										  AppDomain.CurrentDomain.SetData("Scripts", FindScripts());
									  });

				result = tempDomain.GetData("Scripts") as IEnumerable<ScriptInfo>;
			}
			finally
			{
				AppDomain.Unload(tempDomain);
			}

			return result;
		}

		private static IEnumerable<ScriptInfo> FindScripts()
		{
			var result = new List<ScriptInfo>();

			var directory = AppDomain.CurrentDomain.BaseDirectory;

			// Поиск всех исполняемых модулей в каталоге текущего домена приложения
			var assemblyFiles = Directory.EnumerateFiles(directory, "*.dll", SearchOption.TopDirectoryOnly)
										 .Concat(Directory.EnumerateFiles(directory, "*.exe", SearchOption.TopDirectoryOnly));

			foreach (var assemblyFile in assemblyFiles)
			{
				try
				{
					// Загрузка сборки для анализа (ReflectionOnlyLoadFrom не подходит по многим причинам)
					var assembly = Assembly.LoadFrom(assemblyFile);

					// Сборки со строгим именем не рассматриваются (предполагается, что прикладные сборки его не имеют)
					if (!HasPublicKeyToken(assembly))
					{
						var types = assembly.GetTypes();

						foreach (var type in types)
						{
							if (type.IsClass && !type.IsAbstract && !type.IsGenericType && !IsAutogenerated(type))
							{
								var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

								foreach (var method in methods)
								{
									var methodName = method.Name;

									if (!method.IsGenericMethod && !method.IsSpecialName
										&& !string.Equals(methodName, "ToString", StringComparison.Ordinal)
										&& !string.Equals(methodName, "Equals", StringComparison.Ordinal)
										&& !string.Equals(methodName, "GetHashCode", StringComparison.Ordinal)
										&& !string.Equals(methodName, "GetType", StringComparison.Ordinal))
									{
										result.Add(new ScriptInfo
												   {
													   AssemblyPath = assemblyFile,
													   TypeName = type.Name,
													   TypeFullName = type.FullName,
													   MethodName = methodName
												   });
									}
								}
							}
						}
					}
				}
				catch
				{
					// ReflectionTypeLoadException
				}
			}

			return result;
		}

		private static bool HasPublicKeyToken(Assembly assembly)
		{
			var publicKeyToken = assembly.GetName().GetPublicKeyToken();
			return (publicKeyToken != null && publicKeyToken.Length > 0);
		}

		private static bool IsAutogenerated(Type type)
		{
			return (type.GetCustomAttribute(typeof(CompilerGeneratedAttribute), true) != null);
		}

		private static string CombineKey(string term1, string term2)
		{
			return term1 + "," + term2;
		}


		[Serializable]
		private struct ScriptInfo
		{
			public string AssemblyPath;
			public string TypeName;
			public string TypeFullName;
			public string MethodName;
		}
	}
}