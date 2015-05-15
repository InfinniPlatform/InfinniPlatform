namespace InfinniPlatform.Expressions.CompiledExpressions
{
	abstract class UnaryExpression : ICompiledExpression
	{
		protected readonly ICompiledExpression Operand;

		protected UnaryExpression(ICompiledExpression operand)
		{
			Operand = operand;
		}

		public abstract object Execute(object dataContext, ExpressionScope scope);
	}
}