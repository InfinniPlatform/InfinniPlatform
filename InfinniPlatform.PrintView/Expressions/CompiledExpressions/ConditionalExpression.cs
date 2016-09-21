namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class ConditionalExpression : ICompiledExpression
    {
        private readonly ICompiledExpression _condition;
        private readonly ICompiledExpression _whenFalse;
        private readonly ICompiledExpression _whenTrue;

        public ConditionalExpression(ICompiledExpression condition, ICompiledExpression whenTrue,
            ICompiledExpression whenFalse)
        {
            _condition = condition;
            _whenTrue = whenTrue;
            _whenFalse = whenFalse;
        }

        public object Execute(object dataContext, ExpressionScope scope)
        {
            return (bool) _condition.Execute(dataContext, scope)
                ? _whenTrue.Execute(dataContext, scope)
                : _whenFalse.Execute(dataContext, scope);
        }
    }
}