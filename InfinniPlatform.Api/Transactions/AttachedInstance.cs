using System.Collections.Generic;

namespace InfinniPlatform.Api.Transactions
{
    /// <summary>
    ///  Присоединенный к транзакции экземпляр документа
    /// </summary>
	public sealed class AttachedInstance
	{
        /// <summary>
        ///   Идентификатор конфигурации
        /// </summary>
        public string ConfigId { get; set; }

        /// <summary>
        ///   Идентификатор типа документа
        /// </summary>
		public string DocumentId { get; set; }

        /// <summary>
        ///   Список сохраняемых документов
        /// </summary>
		public IEnumerable<dynamic> Documents { get; set; }

        /// <summary>
        ///   Версия конфигурации
        /// </summary>
		public string Version { get; set; }

        /// <summary>
        ///   Роутинг пользователя
        /// </summary>
		public string Routing { get; set; }

        /// <summary>
        ///   Признак отсоединения от транзакции
        /// </summary>
        public bool Detached { get; set; }
	}
}
