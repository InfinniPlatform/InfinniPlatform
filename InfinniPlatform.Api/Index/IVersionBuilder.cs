﻿namespace InfinniPlatform.Api.Index
{
	/// <summary>
	///   Конструктор версий хранилища документов
	/// </summary>
	/// <remarks>
	///   Здесь следует сделать важное замечание: 
	/// Версионирование данных возможно на двух уровнях:
	/// 1. Версионирование на уровне одной записи документа (существование нескольких версий одной и той же записи в одной версии индекса)
	/// 2. Версионирование на уровне индекса (существование нескольких версий индекса одних и тех же матаданных, записи внутри каждого индекса, 
	///  в свою очередь, имеют свои версии уровня 1)
	/// </remarks>
	public interface IVersionBuilder
	{
        /// <summary>
        ///   Проверяет, что версия индекса метаданных существует.
        /// Если в параметрах указан маппинг, то дополнительно будет проверено соответствие имеющиегося маппинга с новым.
        /// Считается, что версия существует, если все свойства переданного маппинга соответсвуют по типу всем имеющимся свойствам,
        /// в противном случае нужно создавать новую версию. 
        /// </summary>
        bool VersionExists(IIndexTypeMapping properties = null);

	    /// <summary>
	    ///   Создать версию индекса метаданных
	    /// </summary>
	    /// <param name="deleteExisting">Флаг, показывающий нужно ли удалять версию харнилища, если она уже существует</param>
	    /// <param name="properties">Первоначальный список полей</param>
        void CreateVersion(bool deleteExisting = false, IIndexTypeMapping properties = null);


	}
}
