using System;
using System.Collections.Generic;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Elements
{
    internal static class ElementExtensions
    {
        public static void ApplyElementMeatadata(this IElement element, dynamic metadata)
        {
            element.SetName(metadata.Name);
            element.SetText(metadata.Text);
            element.SetToolTip(metadata.ToolTip);
            element.SetEnabled(metadata.Enabled != false);
            element.SetVisible(metadata.Visible != false);

            ElementVerticalAlignment verticalAlignment;

            if (Enum.TryParse(metadata.VerticalAlignment as string, true, out verticalAlignment) == false)
            {
                verticalAlignment = ElementVerticalAlignment.Stretch;
            }

            element.SetVerticalAlignment(verticalAlignment);

            ElementHorizontalAlignment horizontalAlignment;

            if (Enum.TryParse(metadata.HorizontalAlignment as string, true, out horizontalAlignment) == false)
            {
                horizontalAlignment = ElementHorizontalAlignment.Stretch;
            }

            element.SetHorizontalAlignment(horizontalAlignment);
        }

        public static void InvokeScript(this IViewChild target, ScriptDelegate script,
            Action<dynamic> initArguments = null)
        {
            if (script != null)
            {
                var view = target.GetView();

                if (view != null)
                {
                    dynamic context = view.GetContext();
                    dynamic arguments = new DynamicWrapper();

                    arguments.Source = target;

                    if (initArguments != null)
                    {
                        initArguments(arguments);
                    }

                    script(context, arguments);
                }
            }
        }

        public static TControl GetControl<TControl>(this IElement element) where TControl : class
        {
            return (element != null) ? element.GetControl() as TControl : null;
        }

        public static IEnumerable<IElement> GetAllChildElements(this IElement element)
        {
            var childElements = element.GetChildElements();

            if (childElements != null)
            {
                var elements = new List<IElement>();

                foreach (var childElement in childElements)
                {
                    FillChildElements(elements, childElement);
                }

                return elements;
            }

            return null;
        }

        private static void FillChildElements(List<IElement> elements, IElement element)
        {
            if (elements.Contains(element) == false)
            {
                elements.Add(element);

                var childElements = element.GetChildElements();

                if (childElements != null)
                {
                    foreach (var childElement in childElements)
                    {
                        FillChildElements(elements, childElement);
                    }
                }
            }
        }
    }
}