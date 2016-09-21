namespace InfinniPlatform.PrintView.Expressions
{
    internal interface ICompiledExpression
    {
        object Execute(object dataContext, ExpressionScope scope);
    }
}