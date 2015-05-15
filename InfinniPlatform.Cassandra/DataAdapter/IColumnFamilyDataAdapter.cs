namespace InfinniPlatform.Cassandra.DataAdapter
{
	/// <summary>
	/// Represents data adapter for the key-value storage.
	/// </summary>
	public interface IColumnFamilyDataAdapter
	{
		/// <summary>
		/// Executes insert data.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		void Insert(InsertQueryStatement statement, object[] parameters, Consistency consistency = Consistency.One);

		/// <summary>
		/// Executes update data.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		void Update(UpdateQueryStatement statement, object[] parameters, Consistency consistency = Consistency.One);

		/// <summary>
		/// Executes delete data.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		void Delete(DeleteQueryStatement statement, object[] parameters, Consistency consistency = Consistency.One);
	}
}