using System;
using DevExpress.XtraEditors.Repository;

namespace InfinniPlatform.DesignControls.PropertyEditors
{
    public interface IPropertyEditor
    {
        Func<string, dynamic> ItemPropertyFunc { get; set; }
        RepositoryItem GetRepositoryItem(object value);
    }
}