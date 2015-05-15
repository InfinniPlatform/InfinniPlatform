using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfinniPlatform.SystemConfig.Configurator
{
	public sealed class ActionUnitRevertMigration
	{
        // Пока имя сборки с классами миграций прописано жестко.
        // Возможно, необходимо вынести это в настройки
        private const string AssemblyName = "InfinniPlatform.MigrationsAndVerifications.dll";

        public void Action(IApplyContext target)
		{
		    string migrationName = target.Item.MigrationName.ToString();
            string configurationName = target.Item.ConfigurationName.ToString();

            // Подготовка параметров миграции
            var parameters = new List<object>();
            if (target.Item.Parameters != null)
            {
                foreach (var parameter in target.Item.Parameters)
                {
                    parameters.Add(parameter);
                }
            }
            
            var assembly = Assembly.Load(
                new AssemblyName
                {
                    CodeBase = AssemblyName
                });

            var migrationClass = assembly.GetTypes().Where(
                t => typeof(IConfigurationMigration).IsAssignableFrom(t))
                .FirstOrDefault(t => t.Name == migrationName);

            if (migrationClass == null)
            {
                target.Result = string.Format("Migration {0} not found.", migrationName);
            }
            else
            {
                var migration = (IConfigurationMigration) Activator.CreateInstance(migrationClass);

                migration.AssignActiveConfiguration(configurationName, target.Context);

                string message;

                migration.Down(out message, parameters.ToArray());
                target.Result = message;
            }
		}
	}
}
