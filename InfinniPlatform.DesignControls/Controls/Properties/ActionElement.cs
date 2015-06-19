using System.Collections.Generic;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.Controls.Properties
{
    public static class ActionElement
    {
        private static ObjectProperty GetPopupButtonProperties(int actionLevel)
        {
            //реализуем меню максимум из трех уровней вложенности
            if (actionLevel > 3)
            {
                return new ObjectProperty(new Dictionary<string, IControlProperty>(),
                    new Dictionary<string, CollectionProperty>());
            }
            return new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"Image", new SimpleProperty(string.Empty)},
                {
                    "Action", new ObjectProperty(Action.GetActions(), new Dictionary<string, CollectionProperty>())
                },
                {
                    "OnClick", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"Name", new SimpleProperty(string.Empty)}
                    }, new Dictionary<string, CollectionProperty>())
                }
            }.InheritBaseElementSimpleProperties(), new Dictionary<string, CollectionProperty>
            {
                {
                    "Items",
                    new CollectionProperty(new Dictionary<string, IControlProperty>().GetActionElements(actionLevel))
                }
            });
        }

        public static Dictionary<string, IControlProperty> GetActionProperties(
            this Dictionary<string, IControlProperty> properties)
        {
            properties.Add("Action",
                new ObjectProperty(Action.GetActions(), new Dictionary<string, CollectionProperty>()));
            return properties;
        }

        public static Dictionary<string, IControlProperty> GetActionElements(
            this Dictionary<string, IControlProperty> properties, int currentActionElementLevel = 0)
        {
            currentActionElementLevel++;
            properties.Add("Button", new ObjectProperty(new Dictionary<string, IControlProperty>
            {
                {"Image", new SimpleProperty(null)},
                {"Action", new ObjectProperty(Action.GetActions(), new Dictionary<string, CollectionProperty>())},
                {
                    "OnClick", new ObjectProperty(new Dictionary<string, IControlProperty>
                    {
                        {"Name", new SimpleProperty(string.Empty)}
                    }, new Dictionary<string, CollectionProperty>())
                }
            }.InheritBaseElementSimpleProperties(), new Dictionary<string, CollectionProperty>()));

            properties.Add("PopupButton", GetPopupButtonProperties(currentActionElementLevel));
            return properties;
        }
    }
}