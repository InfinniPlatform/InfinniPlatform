namespace InfinniPlatform.Expressions.CompiledExpressions
{
    internal abstract class BinaryExpression : ICompiledExpression
    {
        protected readonly ICompiledExpression Left;
        protected readonly ICompiledExpression Right;

        protected BinaryExpression(ICompiledExpression left, ICompiledExpression right)
        {
            Left = left;
            Right = right;
        }

        public abstract object Execute(object dataContext, ExpressionScope scope);
    }
}