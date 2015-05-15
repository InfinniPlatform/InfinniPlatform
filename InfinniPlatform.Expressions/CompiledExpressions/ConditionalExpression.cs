namespace InfinniPlatform.Expressions.CompiledExpressions
{
	sealed class ConditionalExpression : ICompiledExpression
	{
		private readonly ICompiledExpression _condition;
		private readonly ICompiledExpression _whenTrue;
		private readonly ICompiledExpression _whenFalse;

		public ConditionalExpression(ICompiledExpression condition, ICompiledExpression whenTrue, ICompiledExpression whenFalse)
		{
			_condition = condition;
			_whenTrue = whenTrue;
			_whenFalse = whenFalse;
		}

		public object Execute(object dataContext, ExpressionScope scope)
		{
			return (bool)_condition.Execute(dataContext, scope)
				? _whenTrue.Execute(dataContext, scope)
				: _whenFalse.Execute(dataContext, scope);
		}
	}
}