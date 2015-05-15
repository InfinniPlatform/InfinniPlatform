using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetMigrationDetails
    {
        // Пока имя сборки с классами миграций прописано жестко.
        // Возможно, необходимо вынести это в настройки
        private const string AssemblyName = "InfinniPlatform.MigrationsAndVerifications.dll";

        public void Action(IApplyContext target)
        {
            string migrationName = target.Item.MigrationName.ToString();
            string configurationName = target.Item.ConfigurationName.ToString();

            var assembly = Assembly.Load(
                new AssemblyName
                {
                    CodeBase = AssemblyName
                });

            var selectedType = assembly.GetTypes().FirstOrDefault(t => typeof (IConfigurationMigration).IsAssignableFrom(t) && t.Name == migrationName);

            if (selectedType != null)
            {
                var migration = (IConfigurationMigration) Activator.CreateInstance(selectedType);

                migration.AssignActiveConfiguration(configurationName, target.Context);

                target.Result = new
                {
                    selectedType.Name,
                    migration.Description,
                    migration.IsUndoable,
                    migration.ConfigurationId,
                    migration.ConfigVersion,
                    migration.Parameters
                }.ToDynamic();
            }
        }
    }
}
