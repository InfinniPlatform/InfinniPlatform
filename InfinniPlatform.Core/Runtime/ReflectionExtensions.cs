using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.Runtime
{

	public static class ReflectionExtensions
	{
        /// <summary>
        ///   Найти методы сборок
        /// </summary>
        /// <param name="assemblies">Сборки, в которых выполняется поиск</param>
        /// <param name="type">Тип объекта, который ищем</param>
        /// <param name="method">Метод, который ищем</param>
        /// <returns>Список найденных методов</returns>
		public static IEnumerable<MethodInfo> FindMethodInfo(IEnumerable<Assembly> assemblies, string type, string method)
		{
			var result = new List<MethodInfo>();
			foreach (var assembly in assemblies)
			{
				foreach (var assemblyType in assembly.GetTypes())
				{
					if (assemblyType.Name == type)
					{
                        var methods = assemblyType.GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance).ToList();
						foreach (var methodInfo in methods)
						{
							if (methodInfo.Name == method)
							{
								result.Add(methodInfo);
							}
						}
					}
				}
			}
			return result;
		}

	}

}