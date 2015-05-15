using InfinniPlatform.Api.Events;
using System.Collections.Generic;

namespace InfinniPlatform.Api.ContextTypes
{
    /// <summary>
    ///   Контекст обработчика бизнес-логики проведения документа
    /// </summary>
	public interface IApplyContext : ICommonContext
	{
		/// <summary>
		///   Свойства, значения которых должны устанавливаться по умолчанию
		/// </summary>
		List<EventDefinition> DefaultProperties { get; set; }

        /// <summary>
        ///   Список событий
        /// </summary>
        List<EventDefinition> Events { get; set; }

		/// <summary>
		///   Идентификатор обрабатываемого объекта
		/// </summary>
		string Id { get; set; }

		/// <summary>
		///   Наименование индекса
		/// </summary>
		string Type { get; set; }

		/// <summary>
		///   Объект, переводимый в состояние
		/// </summary>
		dynamic Item { get; set; }

		/// <summary>
		///  Статус объекта по окончании операции 
		/// </summary>
		object Status { get; set; }

		/// <summary>
		///   Результат, возвращаемый по окончании обработки
		/// </summary>
		dynamic Result { get; set; }

	    /// <summary>
	    ///   Маркер транзакции используемой при обработке запроса
	    /// </summary>
	    string TransactionMarker { get; set; }

	    /// <summary>
        ///   Сохранить документ
        /// </summary>
        /// <param name="configuration">Идентификатор конфигурации</param>
        /// <param name="metadata">Идентификатор метаданных объекта</param>
        /// <param name="document">Экземпляр сохраняемого документа</param>
        void SetDocument(string configuration, string metadata, dynamic document);
	}
}
