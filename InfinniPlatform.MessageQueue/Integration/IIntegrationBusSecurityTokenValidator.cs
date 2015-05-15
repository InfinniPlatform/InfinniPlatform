namespace InfinniPlatform.MessageQueue.Integration
{
	/// <summary>
	/// Осуществляет проверку маркера безопасности при обращении к интеграционной шине.
	/// </summary>
	interface IIntegrationBusSecurityTokenValidator
	{
		/// <summary>
		/// Осуществить проверку маркера безопасности и бросить исключение, если он недействителен.
		/// </summary>
		/// <param name="securityToken">Маркер безопасности.</param>
		void Validate(string securityToken);
	}
}