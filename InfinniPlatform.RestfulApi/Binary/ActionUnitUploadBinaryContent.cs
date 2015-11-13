using System.IO;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Binary
{
    /// <summary>
    /// Модуль загрузки бинарных данных на сервер
    /// </summary>
    public sealed class ActionUnitUploadBinaryContent
    {
        public void Action(IUploadContext target)
        {
            var documentApi = target.Context.GetComponent<DocumentApi>();
            var blobStorage = target.Context.GetComponent<IBlobStorageComponent>();
            var binaryManager = new BinaryManager(blobStorage.GetBlobStorage());

            string documentId = target.LinkedData.DocumentId;

            var document = documentApi.GetDocument(documentId);

            if (document != null)
            {
                string configuration = target.LinkedData.Configuration;
                string documentType = target.LinkedData.Metadata;
                string blobProperty = target.LinkedData.FieldName;
                byte[] blobData = ReadAsBytes(target.FileContent);

                binaryManager.SaveBinary(new[] { document }, configuration, documentType, blobProperty, blobData);
            }
            else
            {
                target.IsValid = false;
                target.ValidationMessage = string.Format("Document with identifier {0}, not found.", target.LinkedData.InstanceId);
            }
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