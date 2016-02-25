using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Documents.Interceptors
{
    /// <summary>
    /// Набор команд изменения документов в рамках одного запроса к хранилищу.
    /// </summary>
    public sealed class DocumentBulkCommand
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="commands">Набор команд изменения документов.</param>
        /// <param name="isOrdered">Обязательно ли выполнять команды по порядку.</param>
        public DocumentBulkCommand(IEnumerable<IDocumentWriteCommand> commands, bool isOrdered = false)
        {
            Commands = commands;
            IsOrdered = isOrdered;
        }

        /// <summary>
        /// Набор команд изменения документов.
        /// </summary>
        public IEnumerable<IDocumentWriteCommand> Commands { get; }

        /// <summary>
        /// Обязательно ли выполнять команды по порядку.
        /// </summary>
        public bool IsOrdered { get; }
    }


    /// <summary>
    /// Набор команд изменения документов в рамках одного запроса к хранилищу.
    /// </summary>
    public sealed class DocumentBulkCommand<TDocument>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="commands">Набор команд изменения документов.</param>
        /// <param name="isOrdered">Обязательно ли выполнять команды по порядку.</param>
        public DocumentBulkCommand(IEnumerable<IDocumentWriteCommand<TDocument>> commands, bool isOrdered = false)
        {
            Commands = commands;
            IsOrdered = isOrdered;
        }

        /// <summary>
        /// Набор команд изменения документов.
        /// </summary>
        public IEnumerable<IDocumentWriteCommand<TDocument>> Commands { get; }

        /// <summary>
        /// Обязательно ли выполнять команды по порядку.
        /// </summary>
        public bool IsOrdered { get; }
    }
}