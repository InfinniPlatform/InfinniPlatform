using System;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetMigrations
    {
        // Пока имя сборки с классами миграций прописано жестко.
        // Возможно, необходимо вынести это в настройки
        private const string AssemblyName = "InfinniPlatform.MigrationsAndVerifications.dll";

        public void Action(IApplyContext target)
        {
            var assembly = Assembly.Load(
                new AssemblyName
                {
                    CodeBase = AssemblyName
                });

            var selectedTypes = assembly.GetTypes().Where(t => typeof (IConfigurationMigration).IsAssignableFrom(t));

            var result = (from type in selectedTypes
                let migration = (IConfigurationMigration) Activator.CreateInstance(type)
                select new
                {
                    type.Name, 
                    migration.Description, 
                    migration.IsUndoable, 
                    migration.ConfigurationId, 
                    migration.ConfigVersion
                }.ToDynamic()).Cast<dynamic>().ToList();

            target.Result = result;
        }
    }
}
