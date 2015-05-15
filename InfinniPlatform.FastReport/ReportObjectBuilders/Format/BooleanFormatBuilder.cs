using FastReport;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Format
{
	sealed class BooleanFormatBuilder : IReportObjectBuilder<BooleanFormat>
	{
		public void BuildObject(IReportObjectBuilderContext context, BooleanFormat template, object parent)
		{
			var element = (TextObjectBase)parent;

			element.Format = new global::FastReport.Format.BooleanFormat
								 {
									 FalseText = template.FalseText,
									 TrueText = template.TrueText
								 };
		}
	}
}