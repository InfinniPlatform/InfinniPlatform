using System;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.SystemConfig.Configurator
{
	/// <summary>
	///   Поиск объекта метаданных по наименованию, если оно предоставлено в качестве идентификатора
	///   Возвращается первый найденный элемент
	/// </summary>
	public sealed class ActionUnitFilterMetadata
	{
		public void Action(IProcessEventContext target)
		{
			if (target.Id == null)
			{
				return;
			}

			Guid guid;

			if (!Guid.TryParse(target.Id, out guid))
			{

				var eventNameMetadata = target.Id;

			    var managerIdentifiers = target.Context.GetComponent<ISystemComponent>().ManagerIdentifiers;

			    target.Id = managerIdentifiers.GetConfigurationUid(eventNameMetadata);

			}

            dynamic document;

            if (!string.IsNullOrEmpty(target.Id))
            {
                var documentApi = target.Context.GetComponent<DocumentApi>();

                document = documentApi.GetDocument(target.Id);
                if (document != null)
                {
                    target.Item = document;
                }
            }

		}
	}
}
