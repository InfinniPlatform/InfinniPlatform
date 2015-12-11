using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    /// Возвращает печатное представление.
    /// </summary>
    public sealed class ActionUnitGetPrintView
    {
        public ActionUnitGetPrintView(DocumentApi documentApi, IMetadataComponent metadataComponent, IPrintViewComponent printViewComponent)
        {
            _documentApi = documentApi;
            _metadataComponent = metadataComponent;
            _printViewComponent = printViewComponent;
        }

        private readonly DocumentApi _documentApi;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IPrintViewComponent _printViewComponent;

        public void Action(IUrlEncodedDataContext target)
        {
            string configId = target.FormData.ConfigId;
            string documentId = target.FormData.DocumentId;
            string printViewId = target.FormData.PrintViewId;
            dynamic documentFilter = target.FormData.Query;
            var pageNumber = (target.FormData.PageNumber != null) ? (int)target.FormData.PageNumber : 0;
            var pageSize = (target.FormData.PageSize != null) ? (int)target.FormData.PageSize : 10;

            var printViewMetadata = _metadataComponent.GetMetadataItem(configId, documentId, MetadataType.PrintView, (dynamic i) => i.Name == printViewId);

            var data = new byte[0];

            if (printViewMetadata != null)
            {
                var printViewSource = _documentApi.GetDocument(configId, documentId, documentFilter, pageNumber, pageSize);

                // TODO: Это вообще какое-то извращение

                string updateAction = target.FormData.ActionId;

                if (!string.IsNullOrEmpty(updateAction))
                {
                    dynamic requestBody = new DynamicWrapper();
                    requestBody.Parameters = target.FormData;
                    requestBody.PrintViewSource = printViewSource;

                    printViewSource = RestQueryApi.QueryPostJsonRaw(configId, documentId, updateAction, null, requestBody).ToDynamicList();
                }

                data = _printViewComponent.BuildPrintView(printViewMetadata, printViewSource, PrintViewFileFormat.Pdf);
            }

            target.Result = new DynamicWrapper();
            target.Result.Data = data;
            target.Result.Info = new DynamicWrapper();
            target.Result.Info.Size = data.Length;
            target.Result.Info.Type = "application/pdf";
        }
    }
}