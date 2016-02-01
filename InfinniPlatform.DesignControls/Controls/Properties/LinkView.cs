using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;

namespace InfinniPlatform.DesignControls.Controls.Properties
{
    public static class LinkView
    {
        public static Dictionary<string, IControlProperty> GetLinkViews(
            this Dictionary<string, IControlProperty> properties)
        {
            properties.Add("ExistsView", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"ConfigId", new SimpleProperty(string.Empty)},
                {"DocumentId", new SimpleProperty(string.Empty)},
                {"ViewId", new SimpleProperty(string.Empty)}
            }
                .InheritBaseElementSimpleProperties()
                .InheritBaseLinkViewSimpleProperties(),
                new Dictionary<string, CollectionProperty>().InheritBaseLinkViewCollectionProperties(),
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                {
                    {
                        "ConfigId", Common.CreateNullOrEmptyValidator("ExistsView", "ConfigId")
                    },
                    {
                        "DocumentId", Common.CreateNullOrEmptyValidator("ExistsView", "DocumentId")
                    },
                    {
                        "ViewId", Common.CreateNullOrEmptyValidator("ExistsView", "ViewId")
                    }
                }.InheritBaseElementValidators("ExistsView")
                    .InheritBaseLinkViewValidators("ExistsView")
                ));

            properties.Add("AutoView", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"ConfigId", new SimpleProperty(string.Empty)},
                {"DocumentId", new SimpleProperty(string.Empty)},
                {"ViewType", new SimpleProperty(string.Empty)},
                {"MetadataName", new SimpleProperty(string.Empty)}
            }
                .InheritBaseElementSimpleProperties()
                .InheritBaseLinkViewSimpleProperties(),
                new Dictionary<string, CollectionProperty>().InheritBaseLinkViewCollectionProperties(),
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>
                {
                    {
                        "ConfigId", Common.CreateNullOrEmptyValidator("AutoView", "ConfigId")
                    },
                    {
                        "DocumentId", Common.CreateNullOrEmptyValidator("AutoView", "DocumentId")
                    }
                }.InheritBaseElementValidators("AutoView")
                    .InheritBaseLinkViewValidators("AutoView")
                ));
            properties.Add("ChildView", new ObjectProperty(new Dictionary<string, IControlProperty>()
                .InheritBaseElementSimpleProperties()
                .InheritBaseLinkViewSimpleProperties(),
                new Dictionary<string, CollectionProperty>().InheritBaseLinkViewCollectionProperties()
                ));

            properties.Add("InlineView", new ObjectProperty(new Dictionary<string, IControlProperty>()
                .InheritBaseElementSimpleProperties()
                .InheritBaseLinkViewSimpleProperties(),
                new Dictionary<string, CollectionProperty>().InheritBaseLinkViewCollectionProperties()));
            return properties;
        }

        public static Dictionary<string, Func<IPropertyEditor>> InheritViewPropertyEditors(
            this Dictionary<string, Func<IPropertyEditor>> propertyEditors, ObjectInspectorTree inspector)
        {
            propertyEditors.Add("ConfigId", () => new ConfigIdEditor());
            propertyEditors.Add("DocumentId", () => new DocumentIdEditor());
            propertyEditors.Add("ViewType", ValueListEditorExtensions.CreateViewTypeEditor);
            propertyEditors.Add("OpenMode", ValueListEditorExtensions.CreateOpenModeEditor);
            return propertyEditors;
        }
    }
}