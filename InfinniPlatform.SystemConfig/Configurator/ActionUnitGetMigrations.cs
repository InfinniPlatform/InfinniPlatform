using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetMigrations
    {
        // Пока имя сборки с классами миграций прописано жестко.
        // Возможно, необходимо вынести это в настройки
        private const string AssemblyName = "InfinniPlatform.MigrationsAndVerifications.dll";

        public void Action(IApplyContext target)
        {
            Assembly assembly = Assembly.Load(
                new AssemblyName
                    {
                        CodeBase = AssemblyName
                    });

            IEnumerable<Type> selectedTypes =
                assembly.GetTypes().Where(t => typeof (IConfigurationMigration).IsAssignableFrom(t));

            List<dynamic> result = (from type in selectedTypes
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