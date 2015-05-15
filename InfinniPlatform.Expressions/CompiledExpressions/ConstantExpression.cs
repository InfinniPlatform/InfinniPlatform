namespace InfinniPlatform.Expressions.CompiledExpressions
{
	sealed class ConstantExpression : ICompiledExpression
	{
		public static readonly ConstantExpression Null = new ConstantExpression(null);


		private readonly object _value;

		public ConstantExpression(object value)
		{
			_value = value;
		}

		public object Execute(object dataContext, ExpressionScope scope)
		{
			return _value;
		}
	}
}