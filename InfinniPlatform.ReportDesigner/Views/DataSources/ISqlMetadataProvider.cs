using System.Collections.Generic;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
    /// <summary>
    ///     Провайдер метаданных базы данных.
    /// </summary>
    internal interface ISqlMetadataProvider
    {
        bool TryGetConnectionString(out string connectionString);
        IEnumerable<string> GetTableNames(string connectionString);
        DataSourceInfo GetTableDataSource(string connectionString, string tableName);
    }
}