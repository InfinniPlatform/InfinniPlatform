using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Hosting;

namespace InfinniPlatform.Hosting.Implementation.ServiceTemplates
{
	/// <summary>
	///   Конфигурация шаблонов сервисов
	/// </summary>
	public sealed class ServiceTemplateConfiguration : IServiceTemplateConfiguration
	{
		private readonly IDictionary<string,ServiceTemplate> _serviceTemplateList = new Dictionary<string, ServiceTemplate>(); 

		/// <summary>
		///  Зарегистрировать шаблон сервиса
		/// </summary>
		/// <typeparam name="T">Тип класса, предоставляющего сервис</typeparam>
		/// <param name="containerName">Наименование, под который сервис будет зарегистрирован</param>
		/// <param name="methodName">Имя исполянемого метода</param>
		/// <param name="extensionPointHandlerConfig">Инициализатор обработчика запроса</param>
		/// <returns></returns>
		public IServiceTemplateConfiguration RegisterServiceTemplate<T>(string containerName, string methodName,
		                                                           IExtensionPointHandlerConfig extensionPointHandlerConfig = null) where T:class
		{
			var serviceTemplate = new ServiceTemplate(typeof(T), methodName, extensionPointHandlerConfig);
			_serviceTemplateList.Add(containerName.ToLowerInvariant(), serviceTemplate);
			return this;
		}

		/// <summary>
		///   Получить зарегистрированный обработчик запроса для указанного имени контейнера
		/// </summary>
		/// <param name="containerName">Имя зарегистрированного контейера</param>
		/// <returns>Обработчик запроса</returns>
		public IServiceTemplate GetServiceHandler(string containerName)
		{
			return _serviceTemplateList.ContainsKey(containerName.ToLowerInvariant()) ? _serviceTemplateList[containerName.ToLowerInvariant()] : null;
		}

		/// <summary>
		///   Получить список типов классов, реализующих сервисы
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Type> GetRegisteredTypes()
		{
			return _serviceTemplateList.Select(s => s.Value.ServiceInstanceType).ToList();
		}

		/// <summary>
		///   Получить список зарегистрированных шаблонов сервисов
		/// </summary>
		/// <returns>Список информации о шаблонах</returns>
		public IEnumerable<dynamic> GetRegisteredTemplatesInfo()
		{
			var result = new List<dynamic>();
			foreach (var serviceTemplate in _serviceTemplateList)
			{
				dynamic templateInfo = new DynamicWrapper();
				templateInfo.Name = serviceTemplate.Key;
				templateInfo.WorkflowExtensionPoints = new List<dynamic>();
				foreach (var workflowExtensionPoint in serviceTemplate.Value.HandlerConfig.WorkflowExtensionPoints)
				{
					dynamic extensionPoint = new DynamicWrapper();
					extensionPoint.Name = workflowExtensionPoint.Key;
					extensionPoint.ContextType = workflowExtensionPoint.Value;
					extensionPoint.Caption = workflowExtensionPoint.Value.GetContextTypeDisplayByKind();
					templateInfo.WorkflowExtensionPoints.Add(extensionPoint);					
				}
				result.Add(templateInfo);
				
			}
			return result;
		}
	}
}