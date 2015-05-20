using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;
using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.SearchOptions.Builders;

namespace InfinniPlatform.RestfulApi.Binary
{
	/// <summary>
	///   Модуль загрузки двоичного контекта
	/// </summary>
	public sealed class ActionUnitDownloadBinaryContent
	{
		public void Action(IUrlEncodedDataContext target)
		{
            dynamic document = new DocumentApi().GetDocument(target.FormData.InstanceId);

			if (document != null)
			{
                var linkValue = ObjectHelper.GetProperty(document, target.FormData.FieldName);
				if (linkValue != null)
				{
					var blobStorage = target.Context.GetComponent<IBlobStorageComponent>().GetBlobStorage();
					var blobData = blobStorage.GetBlobData(Guid.Parse(linkValue.Info.ContentId));
					target.Result = blobData;
				}
				else
				{
					target.Result = null;
				}
			}
			else
			{
				target.Result = null;
			}
		}
	}
}
