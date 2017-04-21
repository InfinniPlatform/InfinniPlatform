namespace InfinniPlatform.DocumentStorage.QuerySyntax
{
    /// <summary>
    /// Узел синтаксического дерева строки запроса.
    /// </summary>
    public interface IQuerySyntaxNode
    {
        /// <summary>
        /// Осуществляет синтаксический анализ узла.
        /// </summary>
        /// <typeparam name="TResult">Тип результата синтаксического анализа.</typeparam>
        /// <param name="visitor">Объект для проведения синтаксического анализа.</param>
        /// <returns>Результат синтаксического анализа.</returns>
        TResult Accept<TResult>(QuerySyntaxVisitor<TResult> visitor);
    }
}