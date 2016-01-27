using System;
using System.Threading.Tasks;

using InfinniPlatform.Core.Contracts;
using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.PrintView;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.SystemConfig.Services
{
    /// <summary>
    /// Реализует REST-сервис для PrintViewApi.
    /// </summary>
    internal sealed class PrintViewApiHttpService : IHttpService
    {
        public PrintViewApiHttpService(IDocumentApi documentApi, IPrintViewApi printViewApi, IScriptProcessor scriptProcessor)
        {
            _documentApi = documentApi;
            _printViewApi = printViewApi;
            _scriptProcessor = scriptProcessor;
        }

        private readonly IDocumentApi _documentApi;
        private readonly IPrintViewApi _printViewApi;
        private readonly IScriptProcessor _scriptProcessor;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/SystemConfig/UrlEncodedData/reporting";

            builder.Post["/GetPrintView"] = GetPrintView;
        }

        private Task<object> GetPrintView(IHttpRequest request)
        {
            string formString = request.Form.Form;

            dynamic form = null;

            if (!string.IsNullOrWhiteSpace(formString))
            {
                formString = Uri.UnescapeDataString(formString);
                form = JsonObjectSerializer.Default.Deserialize(formString);
            }

            if (form != null)
            {
                string configuration = form.ConfigId;
                string documentType = form.DocumentId;
                string printViewName = form.PrintViewId;
                string actionId = form.ActionId;

                object filterObject = form.Query;
                int pageNumber = form.PageNumber;
                int pageSize = form.PageSize;

                FilterCriteria[] filter = null;

                if (filterObject != null)
                {
                    filter = JsonObjectSerializer.Default.ConvertFromDynamic<FilterCriteria[]>(filterObject);
                }

                pageNumber = Math.Max(pageNumber, 0);
                pageSize = Math.Min(pageSize, 1000);

                var printViewSource = _documentApi.GetDocuments(configuration, documentType, filter, pageNumber, pageSize);

                if (!string.IsNullOrEmpty(actionId))
                {
                    var context = new ActionContext { Item = new DynamicWrapper() };
                    context.Item.Parameters = form;
                    context.Item.PrintViewSource = printViewSource;

                    _scriptProcessor.InvokeScriptByType(actionId, context);

                    printViewSource = context.Result;
                }

                var printView = _printViewApi.Build(configuration, documentType, printViewName, printViewSource);

                Task.FromResult<object>(new StreamHttpResponse(printView ?? new byte[] { }, HttpConstants.PdfContentType));
            }

            return Task.FromResult<object>(null);
        }
    }
}