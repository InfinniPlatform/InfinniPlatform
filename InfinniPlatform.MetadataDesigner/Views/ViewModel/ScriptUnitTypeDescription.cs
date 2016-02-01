using System.Collections.Generic;
using System.Linq;

using DevExpress.XtraEditors.Controls;

namespace InfinniPlatform.MetadataDesigner.Views.ViewModel
{
    public sealed class ScriptUnitTypeDescription
    {
        public string Description { get; set; }
    }


    public static class ScriptUnitTypeDescriptionExtensions
    {
        public static IEnumerable<ImageComboBoxItem> BuildImageComboBoxItems(this IEnumerable<ScriptUnitTypeDescription> descriptions)
        {
            return descriptions.Select(d => new ImageComboBoxItem(d.Description, d.Description));
        }
    }
}