using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetMigrationDetails
    {
        // Пока имя сборки с классами миграций прописано жестко.
        // Возможно, необходимо вынести это в настройки
        private const string AssemblyName = "InfinniPlatform.MigrationsAndVerifications.dll";

        public ActionUnitGetMigrationDetails(IConfigurationMigrationFactory migrationFactory)
        {
            _migrationFactory = migrationFactory;
        }

        private readonly IConfigurationMigrationFactory _migrationFactory;

        public void Action(IApplyContext target)
        {
            string migrationName = target.Item.MigrationName;
            string configurationName = target.Item.ConfigurationName;

            var migration = _migrationFactory.CreateMigration(migrationName);

            if (migration != null)
            {
                migration.AssignActiveConfiguration(configurationName, target.Context);

                target.Result = new
                                {
                                    migrationName,
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