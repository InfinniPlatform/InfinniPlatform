using System;
using System.Security;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.MessageQueue.Properties;


namespace InfinniPlatform.MessageQueue.Integration
{
	/// <summary>
	/// Осуществляет проверку маркера безопасности при обращении к интеграционной шине.
	/// </summary>
	/// <remarks>
	/// Текущая реализация достаточно простая. Маркер безопасности прописывается в конфигурационном файле
	/// системы и является общим для всех организаций. Это не совсем хорошо, но лучше, чем вообще ничего.
	/// В дальнейшем, например, маркер можно назначить каждому участнику информационного обмена. К вопросу 
	/// о том, зачем нужен маркер безопасности. Внешняя информационная система может находиться в той же сети,
	/// что и интеграционная шина, но она не получит к ней доступ до тех пор, пока не узнает маркер безопасности.
	/// </remarks>
	sealed class IntegrationBusSecurityTokenValidator : IIntegrationBusSecurityTokenValidator
	{
		public IntegrationBusSecurityTokenValidator()
		{
			_securityToken = AppSettings.GetValue("IntegrationBusSecurityToken");
		}


		private readonly string _securityToken;


		/// <summary>
		/// Осуществить проверку маркера безопасности и бросить исключение, если он недействителен.
		/// </summary>
		/// <param name="securityToken">Маркер безопасности.</param>
		public void Validate(string securityToken)
		{
			if (string.IsNullOrWhiteSpace(securityToken) || string.Equals(securityToken, _securityToken, StringComparison.Ordinal) == false)
			{
				throw new SecurityException(Resources.IntegrationBusSecurityTokenIsNotValid);
			}
		}
	}
}