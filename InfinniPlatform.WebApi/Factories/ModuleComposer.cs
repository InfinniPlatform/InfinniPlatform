using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Modules;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.WebApi.Factories
{

	/// <summary>
	///   Компоновщик модулей сервера хостинга конфигураций
	/// </summary>
    public class ModuleComposer
    {
		private readonly IEnumerable<Assembly> _assemblies;
        private readonly ContainerBuilder _containerBuilder;
        private readonly Func<IContainer> _container;

		/// <summary>
		///   Компоновщик модулей
		///   Для работы необходимо указать список модулей (assemblies), в которых требуется осуществлять поиск конфигураций,
		/// </summary>

		/// <param name="assemblies"></param>
		/// <param name="containerBuilder"></param>
		/// <param name="container"></param>
		public ModuleComposer(IEnumerable<Assembly> assemblies, ContainerBuilder containerBuilder, Func<IContainer> container) {
			
			_assemblies = assemblies;
            _containerBuilder = containerBuilder;
            _container = container;
            RegisterInstallers();
			RegisterTemplateProviders();
		}

		private void RegisterTemplateProviders()
		{
			_assemblies.ToList().ForEach(
					a => a.GetTypes().Where(t => !t.IsAbstract).ToList().ForEach(f =>
					{
						if (f.GetInterfaces().Contains(typeof(ITemplateInstaller)))
						{
							_templateProviders.Add(f);
							_containerBuilder.RegisterType(f).AsImplementedInterfaces().AsSelf().SingleInstance();
						}
					}));
		}

		private readonly IList<Type> _installers = new List<Type>();
		private readonly IList<Type> _templateProviders = new List<Type>();

		/// <summary>
		///   Регистрируем все установщики модулей, найденные в сборках, переданных при создании компоновщика
		/// </summary>
        private void RegisterInstallers()
        {
            _assemblies.ToList().ForEach(
                a => a.GetTypes().Where(t => !t.IsAbstract).ToList().ForEach(f =>
                {
                    if (f.GetInterfaces().Contains(typeof(IModuleInstaller)))
                    {
                        _installers.Add(f);
                        _containerBuilder.RegisterType(f).AsImplementedInterfaces().AsSelf().SingleInstance();
                    }
                }));

        }

		/// <summary>
		///   Установка модулей. 
		///   Выполняет установку всех модулей, в том числе,
		///   являющихся модулями конфигурации
		/// </summary>
		public IEnumerable<IModule> RegisterModules()
		{
			var result = new List<IModule>();
		    var listInstallers = new List<IModuleInstaller>();
			foreach (var installer in _installers)
			{
				var installerInstance = (IModuleInstaller)_container().Resolve(installer);				
                listInstallers.Add(installerInstance);
			}
		    listInstallers = listInstallers.OrderByDescending(l => l.IsSystem).ToList();

		    foreach (var moduleInstaller in listInstallers)
		    {
                var moduleResult = moduleInstaller.InstallModule();
                result.Add(moduleResult);
		    }

			return result;
		}

		/// <summary>
		///   Регистрация всех необходимых шаблонов обработчиков, предоставляемых модулем
		/// </summary>
		public void RegisterTemplates()
		{
			foreach (var templateProvider in _templateProviders)
			{
				var templateProviderInstance = (ITemplateInstaller) _container().Resolve(templateProvider);
				templateProviderInstance.RegisterTemplates();
			}
		}
    }
}