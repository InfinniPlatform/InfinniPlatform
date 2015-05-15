using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;

namespace InfinniPlatform.DesignControls.Controls.DataSources
{

    public static class BaseElement
    {
        public static Dictionary<string, IControlProperty> InheritBaseDataSourceSimpleProperties(
            this Dictionary<string, IControlProperty> properties)
        {
            properties.Add("Name", new SimpleProperty(string.Empty));
            properties.Add("IdProperty", new SimpleProperty(string.Empty));
            properties.Add("FillCreatedItem", new SimpleProperty(true));
            properties.Add("ValidationErrors", new SimpleProperty(new DynamicWrapper()));
			properties.Add("ValidationWarnings", new SimpleProperty(new DynamicWrapper()));
            properties.Add("OnPageSizeChanged", new ObjectProperty(new Dictionary<string, IControlProperty>()
				                                             {
					                                             {"Name",new SimpleProperty(string.Empty)}
				                                             }, new Dictionary<string, CollectionProperty>()));
            properties.Add("OnSelectedItemChanged", new ObjectProperty(new Dictionary<string, IControlProperty>()
				                                             {
					                                             {"Name", new SimpleProperty(string.Empty)}
				                                             }, new Dictionary<string, CollectionProperty>()));
            properties.Add("OnPropertyFiltersChanged", new ObjectProperty(new Dictionary<string, IControlProperty>()
				                                             {
					                                             {"Name", new SimpleProperty(string.Empty)}
				                                             }, new Dictionary<string, CollectionProperty>()));

            properties.Add("OnItemDeleted", new ObjectProperty(new Dictionary<string, IControlProperty>()
				                                             {
					                                             {"Name", new SimpleProperty(string.Empty)}
				                                             }, new Dictionary<string, CollectionProperty>()));
            properties.Add("OnTextFilterChanged", new ObjectProperty(new Dictionary<string, IControlProperty>()
				                                             {
					                                             {"Name", new SimpleProperty(string.Empty)}
				                                             }, new Dictionary<string, CollectionProperty>()));
            properties.Add("OnItemsUpdated", new ObjectProperty(new Dictionary<string, IControlProperty>()
				                                             {
					                                             {"Name", new SimpleProperty(string.Empty)}
				                                             }, new Dictionary<string, CollectionProperty>()));

            return properties;
        }


		public static Dictionary<string, Func<IPropertyEditor>> InheritBaseDataSourceEditors(
			this Dictionary<string, Func<IPropertyEditor>> editors, ObjectInspectorTree objectInspector)
		{
			editors.Add("FillCreatedItem", () => new BooleanEditor());
			editors.Add("ConfigId", () => new ConfigIdEditor());
			editors.Add("DocumentId", () => new DocumentIdEditor());
			editors.Add("Query", () => new JsonObjectEditor());
			editors.Add("ValidationErrors", () => new JsonObjectEditor());
			editors.Add("ValidationWarnings", () => new JsonObjectEditor());
			editors.Add("OnPageSizeChanged.Name", () => new ScriptIdEditor(objectInspector));
			editors.Add("OnSelectedItemChanged.Name", () => new ScriptIdEditor(objectInspector));
			editors.Add("OnPropertyFiltersChanged.Name", () => new ScriptIdEditor(objectInspector));
			editors.Add("OnItemDeleted.Name", () => new ScriptIdEditor(objectInspector));
			editors.Add("OnTextFilterChanged.Name", () => new ScriptIdEditor(objectInspector));
			editors.Add("OnItemsUpdated.Name", () => new ScriptIdEditor(objectInspector));
			return editors;
		}
    }

}
