using System.Collections.Generic;

namespace InfinniPlatform.MetadataDesigner.Views.Validation
{
    /// <summary>
    /// Описание одного предиката валидационного выражения
    /// </summary>
    public sealed class PredicateDescriptionNode
    {
        public PredicateDescriptionNode(
            PredicateDescriptionType type, 
            string methodName, 
            string[] parametersName,
            object[] predicateParameters)
        {
            ParametersName = parametersName;
            PredicateParameters = predicateParameters;
            MethodName = methodName;
            Type = type;
            Children = new List<PredicateDescriptionNode>();
        }

        public PredicateDescriptionType Type { get; private set; }

        public string MethodName { get; private set; }

        public string[] ParametersName { get; private set; }

        public object[] PredicateParameters { get; private set; }

        public List<PredicateDescriptionNode> Children { get; private set; }
        
        public override string ToString()
        {
            switch (PredicateParameters.Length)
            {
                case 0:
                    return MethodName;
                case 1:
                    return string.Format("{0} ({1})", MethodName, PredicateParameters[0]);
                default:
                    return string.Format("{0} ({1}, ...)", MethodName, PredicateParameters[0]);
            }
        }
    }
}
