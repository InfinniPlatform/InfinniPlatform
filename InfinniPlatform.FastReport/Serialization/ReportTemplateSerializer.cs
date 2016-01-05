using InfinniPlatform.Core.Serialization;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Formats;
using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.FastReport.Serialization
{
	/// <summary>
	/// Предоставляет методы для сериализации шаблона отчета.
	/// </summary>
	public sealed class ReportTemplateSerializer
	{
		public static readonly ReportTemplateSerializer Instance = new ReportTemplateSerializer();


		public ReportTemplateSerializer()
		{
			var knownTypes = new KnownTypesContainer()
				// IElement
				.Add<TextElement>("Text")
				.Add<TableElement>("Table")
				// IDataProviderInfo
				.Add<RegisterDataProviderInfo>("RegisterDataProvider")
				.Add<RestDataProviderInfo>("RestDataProvider")
				.Add<SqlDataProviderInfo>("SqlDataProvider")
				// IParameterValueProviderInfo
				.Add<ParameterConstantValueProviderInfo>("ConstantValues")
				.Add<ParameterDataSourceValueProviderInfo>("DataSourceValues")
				// IDataBind
				.Add<ConstantBind>("ConstantBind")
				.Add<PropertyBind>("PropertyBind")
				.Add<CollectionBind>("CollectionBind")
				.Add<ParameterBind>("ParameterBind")
				.Add<TotalBind>("TotalBind")
				// IFormat
				.Add<BooleanFormat>("BooleanFormat")
				.Add<NumberFormat>("NumberFormat")
				.Add<CurrencyFormat>("CurrencyFormat")
				.Add<PercentFormat>("PercentFormat")
				.Add<DateFormat>("DateFormat")
				.Add<TimeFormat>("TimeFormat")
				.Add<CustomFormat>("CustomFormat");

			_reportSerializer = new JsonObjectSerializer(true, knownTypes);
		}


		private readonly JsonObjectSerializer _reportSerializer;


		/// <summary>
		/// Сериализовать шаблон отчета в массив байт.
		/// </summary>
		public byte[] Serialize(ReportTemplate value)
		{
			return _reportSerializer.Serialize(value);
		}

		/// <summary>
		/// Сериализовать шаблон отчета в динамический объект.
		/// </summary>
		public object SerializeToDynamic(ReportTemplate value)
		{
			return _reportSerializer.ConvertToDynamic(value);
		}


		/// <summary>
		/// Десериализовать шаблон отчета из массива байт.
		/// </summary>
		public ReportTemplate Deserialize(byte[] data)
		{
			return (ReportTemplate)_reportSerializer.Deserialize(data, typeof(ReportTemplate));
		}

		/// <summary>
		/// Десериализовать шаблон отчета из динамического объекта.
		/// </summary>
		public ReportTemplate DeserializeFromDynamic(object data)
		{
			return (ReportTemplate)_reportSerializer.ConvertFromDynamic(data, typeof(ReportTemplate));
		}
	}
}