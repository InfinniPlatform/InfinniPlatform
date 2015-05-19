using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Builders;

namespace InfinniPlatform.RestfulApi.Binary
{
	/// <summary>
	///   Модуль загрузки бинарных данных на сервер
	/// </summary>
	public sealed class ActionUnitUploadBinaryContent
	{
		private byte[] fileContent;

		private static byte[] ReadAllBytes(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

		public void Action(IUploadContext target)
		{
			//ищем документ, для которого необходимо сохранить контент
			Action<FilterBuilder> filter = f =>
			             f.AddCriteria(cr => cr.Property("Id").IsEquals(target.LinkedData.DocumentId));

			IEnumerable<dynamic> docs = new DocumentApi().GetDocument(target.LinkedData.Configuration, target.LinkedData.Metadata, filter , 0,1 );

			dynamic documentWithBinaryField = docs.FirstOrDefault();

			if (documentWithBinaryField == null)
			{
				target.IsValid = false;
				target.ValidationMessage = string.Format("Document from configuration {0}, document type {1}, with identifier {2}, not found.",
					target.LinkedData.Configuration, target.LinkedData.Metadata, target.LinkedData.DocumentId);
				return;
			}

			var blobStorage = target.Context.GetComponent<IBlobStorageComponent>().GetBlobStorage();

			fileContent = ReadAllBytes(target.FileContent);

			var contentId = Guid.NewGuid();
			blobStorage.SaveBlob(contentId, target.LinkedData.FieldName, fileContent);

			dynamic infoBlobProperty = ObjectHelper.GetProperty(documentWithBinaryField, target.LinkedData.FieldName);

		    if (infoBlobProperty == null)
		    {
		        infoBlobProperty = new DynamicWrapper();
                infoBlobProperty.Info = new DynamicWrapper();
		        ObjectHelper.SetProperty(documentWithBinaryField, target.LinkedData.FieldName, infoBlobProperty);		        
		    }
			infoBlobProperty.Info.ContentId = contentId.ToString();

			ObjectHelper.SetProperty(documentWithBinaryField, target.LinkedData.FieldName, infoBlobProperty);

			new DocumentApi().SetDocument(target.LinkedData.Configuration, target.LinkedData.Metadata, documentWithBinaryField);
		}
	}
}
