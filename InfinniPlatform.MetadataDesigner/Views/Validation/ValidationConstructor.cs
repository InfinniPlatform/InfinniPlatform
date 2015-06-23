using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.MetadataDesigner.Views.Validation
{
    /// <summary>
    /// Класс для преобразования дерева валидационных предикатов в динамический объект 
    /// </summary>
    public sealed class ValidationConstructor
    {
        private PredicateDescriptionNode _rootPredicateDescription;

        private PredicateDescriptionNode _currentPredicateDescription;
        
        /// <summary>
        /// Добавление предиката
        /// </summary>
        public void AddValidationDescription(PredicateDescriptionNode predicate)
        {
            if (predicate.Type == PredicateDescriptionType.Root)
            {
                _rootPredicateDescription = predicate;
            }
            else
            {
                _currentPredicateDescription.Children.Add(predicate);
            }

            if (predicate.Type != PredicateDescriptionType.Object &&
                predicate.Type != PredicateDescriptionType.Collection)
            {
                _currentPredicateDescription = predicate;
            }
        }

        /// <summary>
        /// Установка активного предиката, редактирование которого выполняется в текущий момент
        /// </summary>
        public PredicateDescriptionNode ActivePredicate
        {
            get
            {
                return _currentPredicateDescription;
            }
            set
            {
                _currentPredicateDescription = value;
            }
        }

        public PredicateDescriptionNode RootPredicate
        {
            get
            {
                return _rootPredicateDescription;
            }
        }

        /// <summary>
        /// Сформировать валидационное выражение
        /// </summary>
        public dynamic BuildValidationStatement()
        {
            var parentExpression = HandleChilds(RootPredicate.Children);

            dynamic validationOperator = new DynamicWrapper();
            validationOperator.ValidationOperator = parentExpression.First();

            return validationOperator;
        }

        private static IEnumerable<dynamic> HandleChilds(IEnumerable<PredicateDescriptionNode> childs)
        {
            var childElements = new List<DynamicWrapper>();

            foreach (var validationDescription in childs)
            {
                var newChildElement = new DynamicWrapper();
                
                var methodName = validationDescription.MethodName;
                
                var childElementParameters = new DynamicWrapper();
                for (int i = 0; i < validationDescription.ParametersName.Length; i++)
                {
                    childElementParameters[validationDescription.ParametersName[i]] = validationDescription.PredicateParameters[i];
                }

                if (validationDescription.Children.Count > 0)
                {
                    if (methodName == "All" || methodName == "Any")
                    {
                        childElementParameters["Operator"] = HandleChilds(validationDescription.Children).FirstOrDefault();
                    }
                    else
                    {
                        childElementParameters["Operators"] = HandleChilds(validationDescription.Children);
                    }
                }

                if (validationDescription.MethodName.Contains("IsNot"))
                {
                    var methodOperator = new DynamicWrapper();
                    methodOperator[methodName.Replace("IsNot", "Is")] = childElementParameters;

                    var notOperator = new DynamicWrapper();
                    notOperator["Operator"] = methodOperator;

                    if (childElementParameters["Message"] != null)
                    {
                        notOperator["Message"] = childElementParameters["Message"];
                        childElementParameters["Message"] = null;
                    }

                    newChildElement["Not"] = notOperator;
                }
                else
                {
                    newChildElement[methodName] = childElementParameters;
                }

                childElements.Add(newChildElement);
            }

            var elementsToRemove = new List<DynamicWrapper>();
            var newElements = new List<DynamicWrapper>();

            var nodesToAdjust = new[] {"Property", "Collection"};

            foreach (var nodeToAdjust in nodesToAdjust)
            {
                foreach (var element in childElements)
                {
                    if (element[nodeToAdjust] == null)
                    {
                        continue;
                    }

                    elementsToRemove.Add(element);

                    try
                    {
                        var elementToAdjust =
                            element[nodeToAdjust]
                                .ToDynamic()
                                ["Operators"]
                                .ToEnumerable()
                                .First();

                        if (elementToAdjust.And != null)
                        {
                            elementToAdjust["And"]["Property"] =
                                element[nodeToAdjust].ToDynamic().Property;
                        }
                        else
                        {
                            elementToAdjust["Or"]["Property"] =
                                element[nodeToAdjust].ToDynamic().Property;
                        }

                        newElements.Add(elementToAdjust);
                    }
                    catch
                    {
                    }
                }
            }

            foreach (var elementToRemove in elementsToRemove)
            {
                childElements.Remove(elementToRemove);
            }

            childElements.AddRange(newElements);


            return childElements;
        }
    }
}
