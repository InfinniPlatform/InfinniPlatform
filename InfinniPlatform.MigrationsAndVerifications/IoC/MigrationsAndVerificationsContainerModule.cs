using InfinniPlatform.Api.Metadata;
using InfinniPlatform.MigrationsAndVerifications.Migrations;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.MigrationsAndVerifications.IoC
{
    internal sealed class MigrationsAndVerificationsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<AddRegistersMigration>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<AddReportsMigration>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<CalculateTotalsForRegisters>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<ClearTestDataMigration>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<DownloadClassifiersDataMigration>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<EditSortablePropertiesMigration>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<ExportImportDataMigration>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<ImportFederalClassifierMigration>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<ImportLocalClassifierMigration>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<UpdateStoreMigration>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<UploadClassifiersDataMigration>()
                   .As<IConfigurationMigration>()
                   .SingleInstance();

            builder.RegisterType<ConfigurationMigrationFactory>()
                   .As<IConfigurationMigrationFactory>()
                   .SingleInstance();
        }
    }
}