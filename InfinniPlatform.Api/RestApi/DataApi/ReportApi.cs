using InfinniPlatform.Api.Reporting;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public sealed class ReportApi
    {
        private readonly string _version;

        public ReportApi(string version)
        {
            _version = version;
        }

        public dynamic GetReport(string configuration, string templateName, object[] parameters,
            ReportFileFormat reportFileFormat)
        {
            return RestQueryApi.QueryPostJsonRaw("SystemConfig", "Reporting", "getReport", null, new
            {
                Configuration = configuration,
                Template = templateName,
                Parameters = parameters,
                FileFormat = (int) reportFileFormat
            }, _version).Content;
        }
    }
}