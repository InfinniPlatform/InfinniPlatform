using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Controls;

namespace InfinniPlatform.MetadataDesigner.Views.ViewModel
{
	public sealed class ProcessDescription
	{
		public object Id { get; set; }

		public string Name { get; set; }

		public string Caption { get; set; }
	}

	public static class ProcessDescriptionExtensions
	{
		public static IEnumerable<ImageComboBoxItem> BuildImageComboBoxItems(this IEnumerable<ProcessDescription> descriptions)
		{
			return descriptions.Select(d => new ImageComboBoxItem(string.Format("{0} ({1})", d.Name, d.Caption), d));
		}

	}

}
