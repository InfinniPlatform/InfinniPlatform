using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Controls;

namespace InfinniPlatform.MetadataDesigner.Views.ViewModel
{
    public class HandlerDescription
    {
        public string HandlerId { get; set; }

        public string HandlerCaption { get; set; }
    }

    public static class HandlerDescriptionExtensions
    {
        public static IEnumerable<ImageComboBoxItem> BuildImageComboBoxItems(this IEnumerable<HandlerDescription> descriptions)
        {
            return descriptions.Select(d => new ImageComboBoxItem(string.Format("{0}", d.HandlerCaption), d));
        }

    }
}
