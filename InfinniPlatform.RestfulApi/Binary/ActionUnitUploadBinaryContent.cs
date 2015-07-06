using System.IO;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Binary
{
    /// <summary>
    ///     Модуль загрузки бинарных данных на сервер
    /// </summary>
    public sealed class ActionUnitUploadBinaryContent
    {
        private byte[] fileContent;

        private static byte[] ReadAllBytes(Stream input)
        {
            var buffer = new byte[16*1024];
            using (var ms = new MemoryStream())
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
            dynamic documentWithBinaryField =
                target.Context.GetComponent<DocumentApi>().GetDocument(target.LinkedData.InstanceId);

            if (documentWithBinaryField == null)
            {
                target.IsValid = false;
                target.ValidationMessage = string.Format("Document with identifier {0}, not found.",
                                                         target.LinkedData.InstanceId);
                return;
            }

            fileContent = ReadAllBytes(target.FileContent);

            new BinaryManager(target.Context.GetComponent<IBlobStorageComponent>().GetBlobStorage())
                .SaveBinary(new[] {documentWithBinaryField},
                            documentWithBinaryField.__ConfigId,
                            target.Context.GetVersion(documentWithBinaryField.__ConfigId,target.UserName),
                            documentWithBinaryField.__DocumentId,
                            target.LinkedData.FieldName, fileContent);
        }
    }
}