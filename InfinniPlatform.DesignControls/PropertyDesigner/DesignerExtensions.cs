using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    public static class DesignerExtensions
    {
        public static RepositoryItem CreateRepositoryItem(ButtonPressedEventHandler clickAction)
        {
            var repositoryItem = new RepositoryItemButtonEdit();
            var glyphButton = new EditorButton(ButtonPredefines.Glyph);
            glyphButton.Image = Properties.Resources.zoom_16x16;

            repositoryItem.Buttons.Clear();
            repositoryItem.Buttons.Add(glyphButton);
            var ellipsisButton = new EditorButton(ButtonPredefines.Ellipsis);
            repositoryItem.Buttons.Add(ellipsisButton);
            var deleteButton = new EditorButton(ButtonPredefines.Delete);
            repositoryItem.Buttons.Add(deleteButton);
            repositoryItem.ButtonClick += clickAction;
            return repositoryItem;
        }




        public static IEnumerable<dynamic> GetCollection(this object item, string property)
        {
            return item.GetProperty(property).ToEnumerable();
        }

        public static bool IsEmpty(dynamic value)
        {
	        var result = true;

	        if (value != null)
	        {
		        var valueAsString = value.ToString();

		        if (!string.IsNullOrEmpty(valueAsString))
		        {
			        var valueAsDynamic = DynamicWrapperExtensions.ToDynamic(value);

			        if (valueAsDynamic != null)
			        {
				        foreach (var property in valueAsDynamic)
				        {
					        result = false;
							break;
				        }
			        }
		        }
	        }

	        return result;
        }


        public static void SetSimplePropertiesToInstance(this IPropertiesProvider propertiesProvider, dynamic instance)
        {
            SetSimplePropertiesToInstance(propertiesProvider.GetSimpleProperties(), instance);
        }

        public static object GetValue(this Dictionary<string, IControlProperty> properties, string propertyName)
        {
            return properties.ContainsKey(propertyName) ? properties[propertyName].Value : null;
        }

        public static void SetSimplePropertiesToInstance(this Dictionary<string, IControlProperty> simpleProperties, dynamic instance)
        {
            foreach (var simpleProperty in simpleProperties)
            {
                var objectProperty = simpleProperty.Value as ObjectProperty;
                if (objectProperty != null)
                {
                    instance[simpleProperty.Key] = objectProperty.Value;
                }

                else
                {
                    instance[simpleProperty.Key] = simpleProperty.Value.Value;
                }

            }
        }


        public static void SetSimplePropertiesFromInstance(this Dictionary<string, IControlProperty> simpleProperties, dynamic instance)
        {
            foreach (var simpleProperty in simpleProperties.ToList())
            {
                if (instance[simpleProperty.Key] != null)
                {
                    var objectProperty = simpleProperty.Value as ObjectProperty;
                    if (objectProperty == null)
                    {
                        var value = instance[simpleProperty.Key];
                        if (value != null)
                        {
                            simpleProperties[simpleProperty.Key].Value = value;
                        }
                    }
                    else
                    {
                        objectProperty.Value = instance[simpleProperty.Key];
                    }
                }
            }

        }

        public static void SetCollectionPropertiesFromInstance(this Dictionary<string, CollectionProperty> collectionProperties,
                                                   dynamic instance)
        {
            foreach (var collectionProperty in collectionProperties)
            {
                if (instance[collectionProperty.Key] != null)
                {
                    var items = (instance[collectionProperty.Key] as IEnumerable);
                    if (items != null)
                    {
                        collectionProperty.Value.Items = items.OfType<dynamic>().ToList();
                    }
                }
            }
        }

        public static void SetCollectionPropertiesToInstance(this Dictionary<string, CollectionProperty> collectionProperties,
                                                   dynamic instance)
        {
            foreach (var collectionProperty in collectionProperties)
            {
                instance[collectionProperty.Key] = collectionProperty.Value.Items;
            }
        }

    }
}
