namespace InfinniPlatform.Core.Metadata
{
    public interface IConfigurationMigrationFactory
    {
        IConfigurationMigration CreateMigration(string migrationName);
    }
}