using System;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Binary
{
    /// <summary>
    ///     Модуль загрузки двоичного контекта
    /// </summary>
    public sealed class ActionUnitDownloadBinaryContent
    {
        public void Action(IUrlEncodedDataContext target)
        {
            dynamic document =
                target.Context.GetComponent<DocumentApi>(target.Version).GetDocument(target.FormData.InstanceId);

            if (document != null)
            {
                var linkValue = ObjectHelper.GetProperty(document, target.FormData.FieldName);
                if (linkValue != null)
                {
                    var blobStorage =
                        target.Context.GetComponent<IBlobStorageComponent>(target.Version).GetBlobStorage();
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