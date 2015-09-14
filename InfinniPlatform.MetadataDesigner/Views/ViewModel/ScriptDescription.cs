using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Controls;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.MetadataDesigner.Views.ViewModel
{

	public sealed class ScriptDescription
	{
		public string TypeName { get; set; }

		public string MethodName { get; set; }

		public ContextTypeKind ContextTypeCode { get; set; }
	}

	public static class ScriptDescriptionExtensions
	{
		public static IEnumerable<ImageComboBoxItem> BuildImageComboBoxItems(this IEnumerable<ScriptDescription> descriptions)
		{
			return descriptions.Select(d => new ImageComboBoxItem(string.Format("{0}",d.TypeName), d));
		}

	}
}
