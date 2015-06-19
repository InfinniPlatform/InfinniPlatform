using System.Collections.Generic;
using InfinniPlatform.Sdk.Application.Events;

namespace InfinniPlatform.Sdk.Application.Contracts
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
	}
}
