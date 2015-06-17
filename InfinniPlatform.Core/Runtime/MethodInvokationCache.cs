using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfinniPlatform.Runtime
{
	/// <summary>
	///   Кэш вызываемых методов
	/// </summary>
	public sealed class MethodInvokationCache
	{
	    private readonly string _version;
	    private readonly DateTime _timeStamp;
		private List<Assembly> _versionAssemblies;

		private readonly ConcurrentBag<MethodInfo> _methodCache = new ConcurrentBag<MethodInfo>();

        /// <summary>
        ///   Конструктор кэша вызываемых методов точек расширения бизнес-логики
        /// </summary>
        /// <param name="version">Версия конфигурации скриптов</param>
        /// <param name="timeStamp">ВременнАя метка конфигурации</param>
        /// <param name="versionAssemblies">Список сборок, содержащих точки расширения бизнес-логики</param>
        public MethodInvokationCache(string version, DateTime timeStamp, IEnumerable<Assembly> versionAssemblies)
        {
            _version = version;
            _timeStamp = timeStamp;
	        _versionAssemblies = versionAssemblies.ToList();
        }

		/// <summary>
		///   Список закэшированных методов прикладных скриптов
		/// </summary>
	    public IEnumerable<MethodInfo> MethodCache
		{
			get { return _methodCache; }
		}

		/// <summary>
		///   Метка актуальности версии
		/// </summary>
		public DateTime TimeStamp
		{
			get { return _timeStamp; }
		}

        /// <summary>
        ///   Версия конфигурации, соответствующая кэшу
        /// </summary>
	    public string Version
	    {
	        get { return _version; }
	    }

	    private MethodInfo GetMethodInfo(string type, string method)
		{
			return MethodCache.FirstOrDefault(m => m.ReflectedType.Name == type && m.Name == method) ;
		}

		/// <summary>
		///  Очистить кэш методов
		/// </summary>
		/// <param name="type">Тип закэшировнного класса</param>
		/// <param name="method">Кэшированный метод</param>
		public void ClearCache(string type, string method)
		{
            var methodInfo = MethodCache.FirstOrDefault(m => m.DeclaringType.Name == type && m.Name == method);
			if (methodInfo != null)
			{
				_methodCache.TryTake(out methodInfo);
			}
		}


	    private readonly object _assemblyLock = new object();

        /// <summary>
        ///   Добавить версии сборок в кэш для поиска
        /// </summary>
        /// <param name="assemblies">Список сборок для поиска методов</param>
        public void AddVersionAssembly(IEnumerable<Assembly> assemblies)
        {
            lock (_assemblyLock)
            {
                _versionAssemblies =
                    _versionAssemblies.Where(v => assemblies.All(asm => asm.FullName != v.FullName)).ToList();
                _versionAssemblies.AddRange(assemblies);    
            }                
        }

        /// <summary>
		///   Поиск метода версии скрипта
		/// </summary>
		/// <param name="type">Тип класса обработчика</param>
		/// <param name="method">Метод обработчика</param>
		/// <returns></returns>
		public MethodInfo FindMethodInfo(string type, string method)
		{
			//TODO: не очищается кэш методов при обновлении прикладных сборок
			//необходимо разобраться, пока каждый раз ищем рефлексией(
			//var cachedInfo = GetMethodInfo(type, method);
			//if (cachedInfo == null)
			//{
                
                var methodInfo = ReflectionExtensions.FindMethodInfo(_versionAssemblies, type, method);
			//	if (methodInfo != null && methodInfo.Any())
			//	{
			//		CacheMethodInfo(methodInfo);
			//	}
			//	return GetMethodInfo(type, method);
			//}
			//return cachedInfo;
	        return methodInfo.FirstOrDefault();
		}
	}


	public static class MethodInvokationCacheExtensions
	{
		public static void ClearCache(this MethodInvokationCache methodInvokationCache, Assembly assembly)
		{
			var types = assembly.GetTypes();
			foreach (var type in types)
			{
				var methods = type.GetMethods();
				foreach (var methodInfo in methods)
				{
					methodInvokationCache.ClearCache(type.Name, methodInfo.Name);
				}
			}
		}
	}


}
