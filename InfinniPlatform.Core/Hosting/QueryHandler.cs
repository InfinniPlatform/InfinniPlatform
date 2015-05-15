using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestQuery;

namespace InfinniPlatform.Hosting
{
	/// <summary>
	///   Обработчик веб-реквеста
	/// </summary>
	public sealed class QueryHandler : IQueryHandler
	{
		private readonly Type _queryHandlerType;
		private HttpResultHandlerType _httpResultHandlerType;

		private readonly IList<IExtensionPointHandler> _actionHandlers = new List<IExtensionPointHandler>(); 

		public QueryHandler(Type queryHandlerType, HttpResultHandlerType httpResultHandlerType)
		{
			_queryHandlerType = queryHandlerType;
			_httpResultHandlerType = httpResultHandlerType;
		}

		/// <summary>
		///   Обработчик результата выполнения запроса
		/// </summary>
		public HttpResultHandlerType HttpResultHandlerType
		{
			get { return _httpResultHandlerType; }
		}

		/// <summary>
		///   Тип класса, содержащего метод, обрабатывающий запрос
		/// </summary>
		public Type QueryHandlerType
		{
			get { return _queryHandlerType; }
		}

		/// <summary>
		///   Список обработчиков запросов
		/// </summary>
		public IList<IExtensionPointHandler> ActionHandlers
		{
			get { return _actionHandlers; }
		}


		/// <summary>
		///  Установить обработчик действия на запрос
		/// </summary>
		/// <param name="extensionPointHandler"></param>
		/// <returns></returns>
		public IQueryHandler ForAction(IExtensionPointHandler extensionPointHandler)
		{
			ActionHandlers.Add(extensionPointHandler);
			return this;
		}

		/// <summary>
		///  Установить обработчик результата запроса
		/// </summary>
		/// <returns></returns>
		public IQueryHandler SetResultHandler(HttpResultHandlerType resultHandlerType)
		{
		    _httpResultHandlerType = resultHandlerType;
			return this;
		}
	}


}