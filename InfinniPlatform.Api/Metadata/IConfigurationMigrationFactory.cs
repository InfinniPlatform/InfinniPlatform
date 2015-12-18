namespace InfinniPlatform.Api.Metadata
{
    public interface IConfigurationMigrationFactory
    {
        IConfigurationMigration CreateMigration(string migrationName);
    }
}