namespace InfinniPlatform.Expressions.CompiledExpressions
{
	interface IInstanceInitializer
	{
		void Initialize(object instance, object dataContext, ExpressionScope scope);
	}
}