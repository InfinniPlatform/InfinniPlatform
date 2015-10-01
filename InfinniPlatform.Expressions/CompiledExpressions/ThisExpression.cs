namespace InfinniPlatform.Expressions.CompiledExpressions
{
    internal sealed class ThisExpression : ICompiledExpression
    {
        public object Execute(object dataContext, ExpressionScope scope)
        {
            return dataContext;
        }
    }
}