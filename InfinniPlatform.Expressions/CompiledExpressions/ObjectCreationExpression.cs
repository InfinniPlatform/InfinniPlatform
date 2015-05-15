using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Expressions.CompiledExpressions
{
	sealed class ObjectCreationExpression : ICompiledExpression
	{
		private readonly Type _type;
		private readonly IEnumerable<ICompiledExpression> _parameters;
		private readonly IInstanceInitializer _initializer;

		public ObjectCreationExpression(Type type, IEnumerable<ICompiledExpression> parameters, IInstanceInitializer initializer)
		{
			_type = type;
			_parameters = parameters;
			_initializer = initializer;
		}

		public object Execute(object dataContext, ExpressionScope scope)
		{
			var parameters = (_parameters != null) ? _parameters.Select(i => i.Execute(dataContext, scope)).ToArray() : null;

			var result = Activator.CreateInstance(_type, parameters);

			if (_initializer != null)
			{
				_initializer.Initialize(result, dataContext, scope);
			}

			return result;
		}
	}
}