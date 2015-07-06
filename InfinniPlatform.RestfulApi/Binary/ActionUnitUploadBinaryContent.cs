using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Builders;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.RestfulApi.Binary
{
	/// <summary>
	///   Модуль загрузки бинарных данных на сервер
	/// </summary>
	public sealed class ActionUnitUploadBinaryContent
	{
		private byte[] _fileContent;

		private static byte[] ReadAllBytes(Stream input, int maxSize)
		{
			var buffer = new byte[16 * 1024];

		    var totalBytesRead = 0;

			using (var ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
				    totalBytesRead += read;
					ms.Write(buffer, 0, read);

				    if (maxSize != -1 && totalBytesRead > maxSize)
				    {
                        throw new Exception(string.Format(Resources.BinaryContentMaxSizeIsExceed, maxSize));
				    }
				}

				return ms.ToArray();
			}
		}

		public void Action(IUploadContext target)
		{
			//ищем документ, для которого необходимо сохранить контент
			Action<FilterBuilder> filter = f => f.AddCriteria(cr => cr.Property("Id").IsEquals(target.LinkedData.DocumentId));

			IEnumerable<dynamic> docs = new DocumentApi().GetDocument(target.LinkedData.Configuration, target.LinkedData.Metadata, filter , 0,1 );

			dynamic documentWithBinaryField = docs.FirstOrDefault();

			if (documentWithBinaryField == null)
			{
				target.IsValid = false;
				target.ValidationMessage = string.Format("Document from configuration {0}, document type {1}, with identifier {2}, not found.",
					target.LinkedData.Configuration, target.LinkedData.Metadata, target.LinkedData.DocumentId);
				return;
			}

            // Считываем метаданные свойства документа для того, чтобы получить максимальный возможный размер бинарного содержимого
		    var configurationMediatorComponent = target.Context.GetComponent<IConfigurationMediatorComponent>();

            var schema = configurationMediatorComponent
                .ConfigurationBuilder.GetConfigurationObject(target.LinkedData.Configuration)
                .MetadataConfiguration.GetSchemaVersion(target.LinkedData.Metadata);
            
		    var maxFileSize = -1;

		    if (schema != null)
		    {
                var pathParts = target.LinkedData.FieldName.TrimEnd('.', '$').Split('.');

		        dynamic propertyMetadata = null;

		        var schemaProperties = schema.Properties;

		        foreach (var pathPart in pathParts)
		        {
		            propertyMetadata = schemaProperties[pathPart];

		            if (propertyMetadata != null)
		            {
		                if (propertyMetadata.Type == "Object")
		                {
		                    schemaProperties = propertyMetadata.Properties;
		                }
		                else if (propertyMetadata.Type == "Array")
		                {
                            schemaProperties = propertyMetadata.Items.Properties;
		                }
		                else
		                {
		                    break;
		                }
		            }
		            else
		            {
		                break;
		            }
		        }

		        if (propertyMetadata != null &&
                    propertyMetadata.TypeInfo != null &&
                    propertyMetadata.TypeInfo.BinaryLink != null)
		        {
		            if (!int.TryParse(propertyMetadata.TypeInfo.BinaryLink.MaxSize.ToString(), out maxFileSize))
		            {
		                maxFileSize = -1;
		            }
		        }
		    }

			var blobStorage = target.Context.GetComponent<IBlobStorageComponent>().GetBlobStorage();

            _fileContent = ReadAllBytes(target.FileContent, maxFileSize);

			var contentId = Guid.NewGuid();
			blobStorage.SaveBlob(contentId, target.LinkedData.FieldName, _fileContent);

			dynamic infoBlobProperty = ObjectHelper.GetProperty(documentWithBinaryField, target.LinkedData.FieldName);

			infoBlobProperty.Info.ContentId = contentId.ToString();

			ObjectHelper.SetProperty(documentWithBinaryField, target.LinkedData.FieldName, infoBlobProperty);

			new DocumentApi().SetDocument(target.LinkedData.Configuration, target.LinkedData.Metadata, documentWithBinaryField);
		}
	}
}
