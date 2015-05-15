﻿using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Hosting
{
	/// <summary>
	///  Контейнер для регистрации сервисов REST
	/// </summary>
	public interface IRestVerbsContainer {
		

		/// <summary>
		///  Добавить обработчик методов сервиса REST
		/// </summary>
		/// <returns>Контейнер обработчиков REST запросов</returns>
		IRestVerbsContainer AddVerb(QueryHandler queryHandler);

		/// <summary>
		///   Найти обработчик для метода POST сервиса REST
		/// </summary>
		/// <param name="serviceName">Наименование сервиса</param>
		/// <param name="verbArguments">Аргументы обработчика</param>
		/// <returns>Экземпляр работчика</returns>
        TargetDelegate FindVerbPost(string serviceName, IDictionary<string, object> verbArguments);

		/// <summary>
		///  Найти обработчик для метода PUT
		/// </summary>
		/// <param name="serviceName">Наименование сервиса</param>
		/// <param name="verbArguments">Аргументы обработчика</param>
		/// <returns>verb instance</returns>
		TargetDelegate FindVerbPut(string serviceName, IDictionary<string, object> verbArguments);

        /// <summary>
        ///   Найти обработчик для метода GET
        /// </summary>
        /// <param name="serviceName">Наименование сервиса</param>
        /// <param name="verbArguments">Аргументы обработчика</param>
        /// <returns></returns>
	    TargetDelegate FindVerbGet(string serviceName, IDictionary<string, object> verbArguments);

		/// <summary>
		///   Найти обработчик для загрузки файла
		/// </summary>
		/// <param name="serviceName">Наименование сервиса</param>
		/// <param name="linkedData">Связанный объект</param>
		/// <param name="uploadStream">Поток с данными файла</param>
		/// <returns></returns>
		TargetDelegate FindUploadVerb(string serviceName, dynamic linkedData, Stream uploadStream);
	}
}
