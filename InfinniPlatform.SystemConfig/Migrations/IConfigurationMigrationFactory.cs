namespace InfinniPlatform.SystemConfig.Migrations
{
    public interface IConfigurationMigrationFactory
    {
        IConfigurationMigration CreateMigration(string migrationName);
    }
}