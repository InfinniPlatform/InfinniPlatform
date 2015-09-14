using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.Controls.Properties
{
    public static class DataElement
    {
        public static Dictionary<string, IControlProperty> GetDataElements(
            this Dictionary<string, IControlProperty> properties)
        {
            ControlRepository.GetDataControls.Select(c => c.Item1).ToList().ForEach(
                item => properties.Add(item,
                    new ObjectProperty(new Dictionary<string, IControlProperty>().InheritBaseElementSimpleProperties(),
                        new Dictionary<string, CollectionProperty>())));
            return properties;
        }
    }
}