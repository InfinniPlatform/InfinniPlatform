using System;

namespace InfinniPlatform.Api.Hosting
{
	public interface IServiceTemplate
	{
		/// <summary>
		///   Тип класса, предоставляющего сервис
		/// </summary>
		Type ServiceInstanceType { get; }

		/// <summary>
		///   Конфигурация обработчика
		/// </summary>
		IExtensionPointHandlerConfig HandlerConfig { get; }

		/// <summary>
		///   Создать экземпляр сконфигурированного обработчика запросов
		/// </summary>
		/// <returns></returns>
		IExtensionPointHandler Build(Func<string,IExtensionPointHandler> buildActionHandler);
	}
}
