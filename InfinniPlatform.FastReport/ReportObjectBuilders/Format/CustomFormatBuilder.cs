using FastReport;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Format
{
	sealed class CustomFormatBuilder : IReportObjectBuilder<CustomFormat>
	{
		public void BuildObject(IReportObjectBuilderContext context, CustomFormat template, object parent)
		{
			var element = (TextObjectBase)parent;

			element.Format = new global::FastReport.Format.CustomFormat
								 {
									 Format = template.Format
								 };
		}
	}
}