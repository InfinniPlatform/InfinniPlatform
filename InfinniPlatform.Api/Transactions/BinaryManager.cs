using System.Collections.Generic;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.Api.Transactions
{
    /// <summary>
    /// Менеджер управления файловыми ссылками в полях документов
    /// </summary>
    public sealed class BinaryManager
    {
        public BinaryManager(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
            _documentApi = new DocumentApi();
        }


        private readonly IBlobStorage _blobStorage;
        private readonly DocumentApi _documentApi;


        /// <summary>
        /// Сохранить BLOB и установить ссылку на BLOB в документе.
        /// </summary>
        /// <param name="documents">Документы, содержащие ссылку.</param>
        /// <param name="configuration">Имя конфигурации.</param>
        /// <param name="documentType">Тип документа.</param>
        /// <param name="blobProperty">Свойство BLOB.</param>
        /// <param name="blobData">Данные BLOB.</param>
        public void SaveBinary(IEnumerable<dynamic> documents, string configuration, string documentType, string blobProperty, byte[] blobData)
        {
            // TODO: Данный метод никак не анализирует тип BLOB-свойства и всегда воспринимает его, как объект (не массив).
            // Таким образом, отсутствует возможность прикрепления к документу коллекции BLOB.
            // На данный момент, с текущим API, не ясно, как можно адекватно обработать эту ситуацию.
            // Скорей всего, целесообразно выделить методы работы с коллекцией BLOB в документе.

            foreach (var document in documents)
            {
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
        }


        private static object GetContentProperty(object document, string contentPropertyName)
        {
            dynamic contentProperty = ObjectHelper.GetProperty(document, contentPropertyName);

            if (contentProperty == null)
            {
                contentProperty = new DynamicWrapper();
                contentProperty.Info = new DynamicWrapper();

                ObjectHelper.SetProperty(document, contentPropertyName, contentProperty);
            }

            return contentProperty;
        }
    }
}