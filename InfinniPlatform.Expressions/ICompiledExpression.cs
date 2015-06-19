namespace InfinniPlatform.Expressions
{
    public interface ICompiledExpression
    {
        object Execute(object dataContext, ExpressionScope scope);
    }
}