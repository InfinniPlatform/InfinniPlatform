using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.MigrationsAndVerifications.Migrations
{
    internal sealed class ConfigurationMigrationFactory : IConfigurationMigrationFactory
    {
        public ConfigurationMigrationFactory(IEnumerable<IConfigurationMigration> migrations)
        {
            _migrations = migrations ?? Enumerable.Empty<IConfigurationMigration>();
        }

        private readonly IEnumerable<IConfigurationMigration> _migrations;

        public IConfigurationMigration CreateMigration(string migrationName)
        {
            if (!string.IsNullOrEmpty(migrationName))
            {
                return _migrations.FirstOrDefault(i => string.Equals(i.GetType().Name, migrationName, StringComparison.OrdinalIgnoreCase));

            }

            return null;
        }
    }
}