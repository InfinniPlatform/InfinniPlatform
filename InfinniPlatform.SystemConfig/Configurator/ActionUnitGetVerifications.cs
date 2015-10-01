using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetVerifications
    {
        // Пока имя сборки с классами миграций прописано жестко.
        // Возможно, необходимо вынести это в настройки
        private const string AssemblyName = "InfinniPlatform.MigrationsAndVerifications.dll";

        public void Action(IApplyContext target)
        {
            Assembly assembly = Assembly.Load(
                new AssemblyName
                    {
                        CodeBase = AssemblyName
                    });

            IEnumerable<Type> selectedTypes =
                assembly.GetTypes().Where(t => typeof (IConfigurationVerification).IsAssignableFrom(t));

            target.Result = (from verificationType in selectedTypes
                             let verification =
                                 (IConfigurationVerification)
                                 Activator.CreateInstance(verificationType)
                             select new
                                 {
                                     verificationType.Name,
                                     verification.Description,
                                     verification.ConfigurationId,
                                     verification.ConfigVersion
                                 }.ToDynamic()).Cast<dynamic>().ToList();
        }
    }
}