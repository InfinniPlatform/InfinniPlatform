using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.Services.QuerySyntax
{
    /// <summary>
    /// Интерфейс синтаксического анализатора строки запроса.
    /// </summary>
    public interface IQuerySyntaxTreeParser
    {
        /// <summary>
        /// Осуществляет синтаксический анализ строки запроса.
        /// </summary>
        /// <param name="query">Строка запроса.</param>
        IList<IQuerySyntaxNode> Parse(string query);
    }
}