namespace InfinniPlatform.DocumentStorage.Services.QuerySyntax
{
    /// <summary>
    /// Предоставляет интерфейс для проведения синтаксического анализа строки запроса.
    /// </summary>
    /// <typeparam name="TResult">Тип результата синтаксического анализа.</typeparam>
    public abstract class QuerySyntaxVisitor<TResult>
    {
        /// <summary>
        /// Обрабатывает вызов метода.
        /// </summary>
        public virtual TResult VisitInvocationExpression(InvocationQuerySyntaxNode node)
        {
            return DefaultVisit(node);
        }

        /// <summary>
        /// Обрабатывает имя идентификатора.
        /// </summary>
        public virtual TResult VisitIdentifierName(IdentifierNameQuerySyntaxNode node)
        {
            return DefaultVisit(node);
        }

        /// <summary>
        /// Обрабатывает литерал.
        /// </summary>
        public virtual TResult VisitLiteral(LiteralQuerySyntaxNode node)
        {
            return DefaultVisit(node);
        }


        /// <summary>
        /// Обработчик узла по умолчанию.
        /// </summary>
        public virtual TResult DefaultVisit(IQuerySyntaxNode node)
        {
            return default(TResult);
        }


        /// <summary>
        /// Обработчик произвольного узла.
        /// </summary>
        public virtual TResult Visit(IQuerySyntaxNode node)
        {
            if (node != null)
            {
                return node.Accept(this);
            }

            return default(TResult);
        }
    }
}