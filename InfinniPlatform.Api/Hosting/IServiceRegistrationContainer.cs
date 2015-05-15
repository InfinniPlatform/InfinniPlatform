using System;
using System.Collections.Generic;

namespace InfinniPlatform.Api.Hosting
{
	/// <summary>
	///   Контейнер регистрации сервисов
	/// </summary>
	public interface IServiceRegistrationContainer
	{
		/// <summary>
		///  Добавить регистрацию сервиса
		/// </summary>
		/// <param name="metadata">Идентификатор метаданных</param>
		/// <param name="serviceName">Наименование сервиса</param>
		/// <param name="actionHandler">Конструктор обработчика</param>
		/// <param name="registrationAction">Инициализация регистрации</param>
		/// <returns></returns>
		IServiceRegistrationContainer AddRegistration(string metadata, string serviceName, Action<IServiceRegistration> registrationAction = null, Func<string, IExtensionPointHandler> actionHandler = null);

		/// <summary>
		///   Список зарегистрированных сервисов
		/// </summary>
		IEnumerable<IServiceRegistration> Registrations { get; }

		/// <summary>
		///   Идентификатор конфигурации, которой принадлежит контейнер
		/// </summary>
		string MetadataConfigurationId { get; }

		/// <summary>
		///   Получить зарегистрированный сервис
		/// </summary>
		/// <param name="metadata">идентификатор метаданных</param>
		/// <param name="service">наименование обработчика</param>
		/// <returns></returns>
		IServiceRegistration GetRegistration(string metadata, string service);

		/// <summary>
		///  Получить первый экземпляр обработчика зарегистрированный под указанным именем сервиса
		/// </summary>
		/// <param name="metadata"></param>
		/// <param name="serviceInstanceName"></param>
		/// <returns></returns>
		IExtensionPointHandlerInstance GetRegistrationByInstanceName(string metadata, string serviceInstanceName);
	}
}
