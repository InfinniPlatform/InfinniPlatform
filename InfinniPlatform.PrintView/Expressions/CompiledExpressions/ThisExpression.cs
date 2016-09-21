namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class ThisExpression : ICompiledExpression
    {
        public object Execute(object dataContext, ExpressionScope scope)
        {
            return dataContext;
        }
    }
}