using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.Api.Transactions
{
    /// <summary>
    ///   Менеджер управления файловыми ссылками в полях документов
    /// </summary>
    public sealed class BinaryManager
    {
        private readonly IBlobStorage _blobStorage;

        public BinaryManager(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        /// <summary>
        ///   Сохранить бинарные данные и установить ссылку в документе
        /// </summary>
        /// <param name="documents">Документы, содержащие ссылку</param>
        /// <param name="documentType">Тип документа</param>
        /// <param name="fieldName">Наименование поля ссылки в документе</param>
        /// <param name="bytes">Массив байт сохраняемых данных</param>
        /// <param name="application">Приложение</param>
        public void SaveBinary(IEnumerable<dynamic> documents, string application, string documentType, string fieldName, byte[] bytes)
        {
            var contentId = Guid.NewGuid();
            _blobStorage.SaveBlob(contentId, fieldName, bytes);

            foreach (var containingDocument in documents)
            {
                dynamic infoBlobProperty = ObjectHelper.GetProperty(containingDocument, fieldName);

                if (infoBlobProperty == null)
                {
                    infoBlobProperty = new DynamicWrapper();
                    infoBlobProperty.Info = new DynamicWrapper();
                    ObjectHelper.SetProperty(containingDocument, fieldName, infoBlobProperty);
                }
                infoBlobProperty.Info.ContentId = contentId.ToString();

                ObjectHelper.SetProperty(containingDocument, fieldName, infoBlobProperty);

                new DocumentApi(containingDocument.Version).SetDocument(application, documentType, containingDocument);
            }
        }

    }
}
