namespace InfinniPlatform.MessageQueue.Integration
{
    /// <summary>
    /// Осуществляет проверку маркера безопасности при обращении к интеграционной шине.
    /// </summary>
    internal sealed class EmptyIntegrationBusSecurityTokenValidator : IIntegrationBusSecurityTokenValidator
    {
        public EmptyIntegrationBusSecurityTokenValidator()
        {
        }


        /// <summary>
        /// Осуществить проверку маркера безопасности и бросить исключение, если он недействителен.
        /// </summary>
        /// <param name="securityToken">Маркер безопасности.</param>
        public void Validate(string securityToken)
        {
        }
    }
}