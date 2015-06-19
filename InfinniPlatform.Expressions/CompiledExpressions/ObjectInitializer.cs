using System.Collections.Generic;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Expressions.CompiledExpressions
{
    internal sealed class ObjectInitializer : IInstanceInitializer
    {
        private readonly IDictionary<string, ICompiledExpression> _properties;

        public ObjectInitializer(IDictionary<string, ICompiledExpression> properties)
        {
            _properties = properties;
        }

        public void Initialize(object instance, object dataContext, ExpressionScope scope)
        {
            if (instance != null && _properties != null)
            {
                foreach (var property in _properties)
                {
                    var propertyName = property.Key;
                    var propertyValue = property.Value.Execute(dataContext, scope);

                    instance.SetProperty(propertyName, propertyValue);
                }
            }
        }
    }
}