namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class UnaryPlusExpression : UnaryExpression
    {
        public UnaryPlusExpression(ICompiledExpression operand)
            : base(operand)
        {
        }

        public override object Execute(object dataContext, ExpressionScope scope)
        {
            dynamic operand = Operand.Execute(dataContext, scope);
            return +operand;
        }
    }
}