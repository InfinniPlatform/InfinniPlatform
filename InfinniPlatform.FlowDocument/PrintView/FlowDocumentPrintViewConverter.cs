using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Api.PrintView;
using InfinniPlatform.FlowDocument.Converters;
using InfinniPlatform.FlowDocument.Converters.Html;
using InfinniPlatform.FlowDocument.Converters.Pdf;
using InfinniPlatform.FlowDocument.Model.Views;
using InfinniPlatform.FlowDocument.Properties;

namespace InfinniPlatform.FlowDocument.PrintView
{
	public sealed class FlowDocumentPrintViewConverter : IFlowDocumentPrintViewConverter
	{
		static FlowDocumentPrintViewConverter()
		{
			DocumentConverters = new Dictionary<PrintViewFileFormat, IFlowDocumentConverter>();
			DocumentConverters.Add(PrintViewFileFormat.Pdf, new FlowDocumentPdfConverter());
			DocumentConverters.Add(PrintViewFileFormat.Html, new FlowDocumentHtmlConverter());
		}


		private static readonly Dictionary<PrintViewFileFormat, IFlowDocumentConverter> DocumentConverters;


		public bool CanConvert(PrintViewFileFormat printViewFileFormat)
		{
			return DocumentConverters.ContainsKey(printViewFileFormat);
		}

		public void Convert(PrintViewDocument printView, Stream printViewStream, PrintViewFileFormat printViewFileFormat)
		{
			IFlowDocumentConverter documentConverter;

			if (DocumentConverters.TryGetValue(printViewFileFormat, out documentConverter))
			{
				documentConverter.Convert(printView, printViewStream);
			}
			else
			{
				throw new NotSupportedException(string.Format(Resources.PrintViewFileFormatIsNotSupported, printViewFileFormat));
			}
		}
	}
}