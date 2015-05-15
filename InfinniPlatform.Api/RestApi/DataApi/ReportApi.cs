using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Reporting;
using InfinniPlatform.Api.RestApi.CommonApi;

namespace InfinniPlatform.Api.RestApi.DataApi
{
	public sealed class ReportApi
	{
		public dynamic GetReport(string configuration, string templateName, object[] parameters, ReportFileFormat reportFileFormat)
		{
			return RestQueryApi.QueryPostJsonRaw("SystemConfig", "Reporting", "getReport", null, new
			{
				Configuration = configuration,
				Template = templateName,
				Parameters = parameters,
				FileFormat = (int)reportFileFormat
			}).Content;
		}
	}
}
