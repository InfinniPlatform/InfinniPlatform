using System;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitRunVerification
    {
        // Пока имя сборки с классами миграций прописано жестко.
        // Возможно, необходимо вынести это в настройки
        private const string AssemblyName = "InfinniPlatform.MigrationsAndVerifications.dll";

        public void Action(IApplyContext target)
        {
            string verificationName = target.Item.VerificationName.ToString();
            string configurationName = target.Item.ConfigurationName.ToString();

            Assembly assembly = Assembly.Load(
                new AssemblyName
                    {
                        CodeBase = AssemblyName
                    });

            Type verificationClass = assembly.GetTypes().Where(
                t => typeof (IConfigurationVerification).IsAssignableFrom(t))
                                             .FirstOrDefault(t => t.Name == verificationName);

            if (verificationClass == null)
            {
                target.Result = string.Format("Verification {0} not found.", verificationName);
            }
            else
            {
                var migration = (IConfigurationVerification) Activator.CreateInstance(verificationClass);

                migration.AssignActiveConfiguration(null, configurationName, target.Context);

                string message;

                migration.Check(out message);

                target.Result = message;
            }
        }
    }
}