namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
	/// <summary>
	/// Типы поставщиков данных.
	/// </summary>
	enum DataSourceProviderType
	{
		/// <summary>
		/// InfinniPlatform Register.
		/// </summary>
		Register = 0,

		/// <summary>
		/// REST Service.
		/// </summary>
		RestService = 1,

		/// <summary>
		/// MS SQL Server.
		/// </summary>
		MsSqlServer = 2,

		/// <summary>
		/// Firebird.
		/// </summary>
		Firebird = 4,
	}
}