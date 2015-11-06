using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Controls;

namespace InfinniPlatform.MetadataDesigner.Views.ViewModel
{
	public class ExtensionPoint
	{
		public string Name { get; set; }

		public int ContextType { get; set; }

		public string Caption { get; set; }

		public ExtensionPoint(string name, int contextType, string caption)
		{
			Name = name;
			ContextType = contextType;
			Caption = caption;
		}
	}

    public static class ExtensionPointExtensions
    {
        public static IEnumerable<ImageComboBoxItem> BuildImageComboBoxItems(this IEnumerable<ExtensionPoint> descriptions)
        {
            return descriptions.Select(d => new ImageComboBoxItem(string.Format("{0} ({1})", d.Caption, d.Name), d));
        }

    }
}
