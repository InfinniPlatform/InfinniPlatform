using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Controls;

using InfinniPlatform.Core.Hosting;

namespace InfinniPlatform.MetadataDesigner.Views.ViewModel
{
	public sealed class ContextTypeDescription
	{
		public string Description { get; set; }

		public ContextTypeKind ContextTypeKind { get; set; }
	}

	public static class ContextTypeDescriptionExtensions
	{
		public static IEnumerable<ImageComboBoxItem> BuildImageComboBoxItems(this IEnumerable<ContextTypeDescription> descriptions)
		{
			return descriptions.Select(d => new ImageComboBoxItem(d.Description, d.ContextTypeKind));
		}
		
	}

}
