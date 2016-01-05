using System;
using System.Collections.Generic;
using System.Reflection;

namespace InfinniPlatform.Core.Runtime
{
	/// <summary>
	/// Кэш прикладный скриптов конфигурации для вызова точек расширения бизнес-логики.
	/// </summary>
	/// <remarks>
	/// Предполагается, что для каждой версии конфигурации - свой кэш прикладных скриптов.
	/// </remarks>
	public interface IMethodInvokationCache
	{
		/// <summary>
		/// Версия конфигурации.
		/// </summary>
		string Version { get; }

		/// <summary>
		/// Метка актуальности версии конфигурации.
		/// </summary>
		DateTime TimeStamp { get; }

		/// <summary>
		/// Добавляет прикладные сборки в кэш.
		/// </summary>
		void AddVersionAssembly(IEnumerable<Assembly> assemblies);

		/// <summary>
		/// Возвращает метод для вызова прикладного скрипта.
		/// </summary>
		/// <param name="type">Имя типа прикладного обработчика.</param>
		/// <param name="method">Имя метода прикладного обработчика.</param>
		MethodInfo FindMethodInfo(string type, string method);
	}
}