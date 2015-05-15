namespace InfinniPlatform.Cassandra
{
	/// <summary>
	/// Represents session factory.
	/// </summary>
	interface ISessionFactory
	{
		/// <summary>
		/// Creates a new session.
		/// </summary>
		ISession OpenSession();
	}
}