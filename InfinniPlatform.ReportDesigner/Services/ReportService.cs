using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using InfinniPlatform.Api.Reporting;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Reports;
using InfinniPlatform.Reporting.Services;

namespace InfinniPlatform.ReportDesigner.Services
{
    // Todo: Удалить зависимость от серверной сборки InfinniPlatform.Reporting

    internal sealed class ReportService
    {
        private static readonly ReportServiceFactory ReportServiceFactory = new ReportServiceFactory();
        public string Address { get; set; }

        public IDictionary<string, ParameterValues> GetParameterValues(ReportTemplate template)
        {
            //Thread.Sleep(1000);

            //var result = new Dictionary<string, ParameterValues>();

            //if (template.Parameters != null)
            //{
            //	foreach (var parameterInfo in template.Parameters)
            //	{
            //		var availableValues = new Dictionary<string, object>();
            //		var defaultValues = new Dictionary<string, object>();

            //		var availableValuesInfo = parameterInfo.AvailableValues as ParameterConstantValueProviderInfo;
            //		var defaultValuesInfo = parameterInfo.DefaultValues as ParameterConstantValueProviderInfo;

            //		if (availableValuesInfo != null)
            //		{
            //			foreach (var item in availableValuesInfo.Items)
            //			{
            //				var label = item.Key;
            //				var value = ((ConstantBind)item.Value).Value;
            //				availableValues.Add(label, value);
            //			}
            //		}

            //		if (defaultValuesInfo != null)
            //		{
            //			foreach (var item in defaultValuesInfo.Items)
            //			{
            //				var label = item.Key;
            //				var value = ((ConstantBind)item.Value).Value;
            //				defaultValues.Add(label, value);
            //			}
            //		}

            //		result.Add(parameterInfo.Name, new ParameterValues
            //										   {
            //											   AvailableValues = availableValues,
            //											   DefaultValues = defaultValues
            //										   });
            //	}
            //}

            //return result;

            //return ReportServiceFactory.CreateReportService().GetParameterValues(template);

            return null;
        }

        public byte[] CreateReportFile(ReportTemplate template, IDictionary<string, object> parameterValues = null,
            ReportFileFormat fileFormat = ReportFileFormat.Pdf)
        {
            // Todo: Здесь должно быть обращение к серверу через REST API
            var data = ReportServiceFactory.CreateReportService()
                .CreateReportFile(template, parameterValues, fileFormat);
            var file = "Temp." + fileFormat;

            File.WriteAllBytes(file, data);

            Process.Start(file);

            return null;
        }
    }
}