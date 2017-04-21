using System;

namespace InfinniPlatform.DocumentStorage.Transactions
{
    /// <summary>
    /// Действие по изменению документов в рамках <see cref="UnitOfWork"/>.
    /// </summary>
    internal class UnitOfWorkItem
    {
        public UnitOfWorkItem(Type type, string name, Action<object> action)
        {
            Type = type;
            Name = name;
            Action = action;
        }

        /// <summary>
        /// Тип документа.
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// Имя типа документа.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Действие изменения документов.
        /// </summary>
        public readonly Action<object> Action;
    }
}