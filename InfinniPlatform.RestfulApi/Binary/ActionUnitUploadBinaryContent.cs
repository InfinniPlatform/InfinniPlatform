using System.IO;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.RestfulApi.Binary
{
    /// <summary>
    /// Модуль загрузки бинарных данных на сервер
    /// </summary>
    public sealed class ActionUnitUploadBinaryContent
    {
        public ActionUnitUploadBinaryContent(DocumentApi documentApi, IBlobStorage blobStorage)
        {
            _documentApi = documentApi;
            _blobStorage = blobStorage;
        }


        private readonly DocumentApi _documentApi;
        private readonly IBlobStorage _blobStorage;


        public void Action(IUploadContext target)
        {
            string documentId = target.LinkedData.DocumentId;

            var document = _documentApi.GetDocument(documentId);

            if (document != null)
            {
                string configuration = target.LinkedData.Configuration;
                string documentType = target.LinkedData.Metadata;
                string blobProperty = target.LinkedData.FieldName;
                var blobData = ReadAsBytes(target.FileContent);

                AttachFileToScalarProperty(document, configuration, documentType, blobProperty, blobData);
            }
            else
            {
                target.IsValid = false;
                target.ValidationMessage = string.Format("Document with identifier {0}, not found.", target.LinkedData.InstanceId);
            }
        }

        private void AttachFileToScalarProperty(object document, string configuration, string documentType, string blobProperty, byte[] blobData)
        {
            // TODO: Данный метод никак не анализирует тип BLOB-свойства и всегда воспринимает его, как не массив.
            // Таким образом, отсутствует возможность прикрепления к документу коллекции BLOB.
            // На данный момент, с текущим API, не ясно, как можно адекватно обрабатывать массивы BLOB.
            // Коллекцию файлов можно хранить, как коллекцию объектов, каждый из которых ссылается на BLOB.

            dynamic contentProperty = GetContentProperty(document, blobProperty);

            string contentId = contentProperty.Info.ContentId;

            // Совершенно не ясно, зачем вместо имени BLOB сохраняется имя свойства - blobProperty.
            // Однако очевидно, что имя BLOB носит исключительно информативный характер.

            if (string.IsNullOrEmpty(contentId))
            {
                contentId = _blobStorage.CreateBlob(blobProperty, string.Empty, blobData);

                contentProperty.Info.ContentId = contentId;
            }
            else
            {
                _blobStorage.UpdateBlob(contentId, blobProperty, string.Empty, blobData);
            }

            _documentApi.SetDocument(configuration, documentType, document);
        }

        private static object GetContentProperty(object document, string contentPropertyName)
        {
            dynamic contentProperty = document.GetProperty(contentPropertyName);

            if (contentProperty == null)
            {
                contentProperty = new DynamicWrapper();
                contentProperty.Info = new DynamicWrapper();

                ObjectHelper.SetProperty(document, contentPropertyName, contentProperty);
            }

            return contentProperty;
        }

        private static byte[] ReadAsBytes(Stream input)
        {
            using (var reader = new MemoryStream())
            {
                input.CopyTo(reader);
                reader.Flush();

                return reader.ToArray();
            }
        }
    }
}