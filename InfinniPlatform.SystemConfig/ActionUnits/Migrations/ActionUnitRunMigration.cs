using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.ActionUnits.Migrations
{
    public sealed class ActionUnitRunMigration
    {
        public ActionUnitRunMigration(IConfigurationMigrationFactory migrationFactory)
        {
            _migrationFactory = migrationFactory;
        }

        private readonly IConfigurationMigrationFactory _migrationFactory;

        public void Action(IApplyContext target)
        {
            string migrationName = target.Item.MigrationName;

            var migration = _migrationFactory.CreateMigration(migrationName);

            if (migration == null)
            {
                target.Result = $"Migration {migrationName} not found.";
            }
            else
            {
                string configurationName = target.Item.ConfigurationName;

                migration.AssignActiveConfiguration(configurationName);

                string message;

                // Подготовка параметров миграции
                var parameters = new List<object>();

                if (target.Item.Parameters != null)
                {
                    foreach (var parameter in target.Item.Parameters)
                    {
                        parameters.Add(parameter);
                    }
                }

                migration.Up(out message, parameters.ToArray());

                target.Result = message;
            }
        }
    }
}