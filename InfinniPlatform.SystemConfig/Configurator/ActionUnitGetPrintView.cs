using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.PrintView;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///     Возвращает печатное представление.
    /// </summary>
    public sealed class ActionUnitGetPrintView
    {
        public void Action(IUrlEncodedDataContext target)
        {
            dynamic parameters = new DynamicWrapper();
            var formDataParameters = target.FormData;
            foreach (var parameter in formDataParameters)
            {
                parameters[parameter.Key] = parameter.Value;
            }

            Func<dynamic, bool> printViewSelector =
                (f) => f.Name == parameters.PrintViewId && f.ViewType == parameters.PrintViewType;

            dynamic printViewMetadata =
                target.Context.GetComponent<IMetadataComponent>()
                      .GetMetadataItem(null, parameters.ConfigId, parameters.DocumentId,
                                       MetadataType.PrintView, printViewSelector);

            int pageNumber = parameters.PageNumber != null ? (int) parameters.PageNumber : 0;
            int pageSize = parameters.PageNumber != null ? (int) parameters.PageNumber : 10;
            string configId = parameters.ConfigId;
            string documentId = parameters.DocumentId;
            string updateAction = parameters.ActionId;

            IEnumerable<dynamic> printViewSource = new DocumentApi().GetDocument(configId, documentId,
                                                                                               parameters.Query,
                                                                                               pageNumber, pageSize);

            dynamic context = new DynamicWrapper();
            context.Parameters = parameters;
            context.PrintViewSource = printViewSource;

            if (!string.IsNullOrEmpty(updateAction))
            {
                printViewSource =
                    RestQueryApi.QueryPostJsonRaw(configId, documentId, updateAction, null, context)
                                .ToDynamicList();
            }


            var data = new byte[0];
            if (printViewMetadata != null)
            {
                data = target.Context.GetComponent<IPrintViewComponent>()
                             .BuildPrintView(printViewMetadata, printViewSource,
                                             PrintViewFileFormat.Pdf);
            }

            target.Result = new DynamicWrapper();
            target.Result.Data = data;
            target.Result.Info = new DynamicWrapper();
            target.Result.Info.Size = data.Length;
            target.Result.Info.Type = "application/pdf";
        }
    }
}