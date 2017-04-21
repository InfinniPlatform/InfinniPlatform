using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Указатель на список документов.
    /// </summary>
    public interface IDocumentCursor<TDocument>
    {
        /// <summary>
        /// Проверят, содержит ли указатель какой-либо элемент.
        /// </summary>
        bool Any();

        /// <summary>
        /// Проверят, содержит ли указатель какой-либо элемент.
        /// </summary>
        Task<bool> AnyAsync();


        /// <summary>
        /// Возвращает первый документ в указателе или исключение, если указатель пуст.
        /// </summary>
        TDocument First();

        /// <summary>
        /// Возвращает первый документ в указателе или исключение, если указатель пуст.
        /// </summary>
        Task<TDocument> FirstAsync();


        /// <summary>
        /// Возвращает первый документ в указателе или значение по умолчанию.
        /// </summary>
        TDocument FirstOrDefault();

        /// <summary>
        /// Возвращает первый документ в указателе или значение по умолчанию.
        /// </summary>
        Task<TDocument> FirstOrDefaultAsync();


        /// <summary>
        /// Возвращает список всех документов указателя.
        /// </summary>
        List<TDocument> ToList();

        /// <summary>
        /// Возвращает список всех документов указателя.
        /// </summary>
        Task<List<TDocument>> ToListAsync();
    }
}