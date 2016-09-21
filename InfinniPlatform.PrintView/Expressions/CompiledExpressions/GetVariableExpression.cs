namespace InfinniPlatform.PrintView.Expressions.CompiledExpressions
{
    internal class GetVariableExpression : ICompiledExpression
    {
        private readonly string _name;

        public GetVariableExpression(string name)
        {
            _name = name;
        }

        public object Execute(object dataContext, ExpressionScope scope)
        {
            return scope.GetVariable(_name);
        }
    }
}