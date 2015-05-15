
namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
    ///   Константы при использовании контрактов, связанных с ElasticSearch
    /// </summary>
	public static class ElasticConstants
	{
        /// <summary>
        ///   Путь к содержимому объекта индекса: Values.
        /// </summary>
        public const string IndexObjectPath = "Values.";
        
        /// <summary>
        ///   Путь к полю временнОй метки индексируемого агрегата: TimeStamp
        /// </summary>
	    public const string IndexObjectTimeStampField = "TimeStamp";

        /// <summary>
        ///   Наименование поля статуса документа
        /// </summary>
        public const string IndexObjectStatusField = "Status";

		/// <summary>
		///   Наименование поля версии документа: Version
		/// </summary>
	    public const string VersionField = "Version";

        /// <summary>
        ///   Наименование поля идентификатора документа: Id
        /// </summary>
        public const string IndexObjectIdentifierField = "Id";
	}
}