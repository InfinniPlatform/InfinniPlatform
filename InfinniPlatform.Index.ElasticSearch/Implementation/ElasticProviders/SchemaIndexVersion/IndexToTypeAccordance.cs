using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{

    /// <summary>
    ///   Соответствие индексов и хранящихся в них типов
    ///   Например:
    ///   Индекс: product_schema_0
    ///   Типы:      customer_schema_0
    ///              customer_schema_1
    ///              customer_schema_2
    ///           product_schema_1
    ///              customer_schema_3
    ///              product_schema_0
    ///              order_schema_0
    ///              order_schema_1      
    /// </summary>
    public sealed class IndexToTypeAccordance
    {
	    private readonly string _indexName;

	    /// <summary>
		///   Базовое наименование типа данных (например product, order, etc)
		/// для которого существуют производные типы с собственными маппингами (product_schema_0, order_schema_1, etc)
		/// </summary>
	    private readonly string _baseTypeName;

		private readonly Dictionary<string, object> _typeNames = new Dictionary<string, object>();

	    public IndexToTypeAccordance(string indexName, string baseTypeName)
	    {
		    _indexName = indexName;
		    _baseTypeName = baseTypeName;
	    }

	    public string IndexName
	    {
		    get { return _indexName; }
	    }

	    /// <summary>
	    ///   Наименования схем типов, отсортированные в порядке убывания (product_schema_1, product_schema_0, etc)
	    /// </summary>
	    public IEnumerable<string> TypeNames
	    {
		    get { return _typeNames.Select(t => t.Key).ToList(); }
	    }

	    /// <summary>
	    ///   Базовое наименование типа данных (например product, order, etc)
	    /// для которого существуют производные типы с собственными маппингами (product_schema_0, order_schema_1, etc)
	    /// </summary>
	    public string BaseTypeName
	    {
		    get { return _baseTypeName; }
	    }

	    /// <summary>
	    ///   Добавить версию схемы типа
	    /// (например product_schema_1)
	    /// </summary>
	    /// <param name="schemaTypeVersion">Наименование версии схемы типа</param>
	    /// <param name="mapping">Схема маппинга версии типа</param>
	    public void RegisterSchemaType(string schemaTypeVersion, object mapping)
	    {
		    _typeNames.Add(schemaTypeVersion,mapping);
	    }
    }
}
