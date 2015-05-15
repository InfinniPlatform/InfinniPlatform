using System.Collections.Generic;

namespace InfinniPlatform.Expressions.CompiledExpressions
{
	sealed class SwitchExpression : ICompiledExpression
	{
		private readonly ICompiledExpression _expression;
		private readonly IDictionary<ICompiledExpression, ICompiledExpression> _sections;

		public SwitchExpression(ICompiledExpression expression, IDictionary<ICompiledExpression, ICompiledExpression> sections)
		{
			_expression = expression;
			_sections = sections;
		}

		public object Execute(object dataContext, ExpressionScope scope)
		{
			var expression = _expression.Execute(dataContext, scope);

			foreach (var section in _sections)
			{
				var label = section.Key.Execute(dataContext, scope);

				if (Equals(label, expression))
				{
					return section.Value.Execute(dataContext, scope);
				}
			}

			return null;
		}
	}
}