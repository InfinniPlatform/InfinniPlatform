namespace InfinniPlatform.Expressions.CompiledExpressions
{
    internal interface IInstanceInitializer
    {
        void Initialize(object instance, object dataContext, ExpressionScope scope);
    }
}