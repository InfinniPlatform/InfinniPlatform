namespace InfinniPlatform.Expressions.CompiledExpressions
{
    internal sealed class UnaryMinusExpression : UnaryExpression
    {
        public UnaryMinusExpression(ICompiledExpression operand)
            : base(operand)
        {
        }

        public override object Execute(object dataContext, ExpressionScope scope)
        {
            dynamic operand = Operand.Execute(dataContext, scope);
            return -operand;
        }
    }
}