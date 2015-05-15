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
			Action<FilterBuilder> builder = f => f.AddCriteria(cr => cr.Property("Id").IsEquals(target.FormData.DocumentId));
			IEnumerable<dynamic> documents = new DocumentApi().GetDocument(target.FormData.Configuration, target.FormData.Metadata, builder, 0,1);
			dynamic document = documents.FirstOrDefault();

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
