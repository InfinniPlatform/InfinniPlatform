using InfinniPlatform.Api.Hosting;

namespace InfinniPlatform.Api.TestEnvironment
{
	public class TestServerParametersBuilder
	{
		public TestServerParametersBuilder()
		{
			_serverConfiguration = new TestServerParameters();
		}


		private readonly TestServerParameters _serverConfiguration;


		/// <summary>
		/// Настройки подсистемы хостинга.
		/// </summary>
		public TestServerParametersBuilder SetHostingConfig(HostingConfig hostingConfig)
		{
			_serverConfiguration.HostingConfig = hostingConfig;
			return this;
		}


		/// <summary>
		/// Перенаправить вывод консоли в файл.
		/// </summary>
		public TestServerParametersBuilder SetOutputToFile()
		{
			_serverConfiguration.RedirectConsoleToFileOutput = true;
			return this;
		}

		/// <summary>
		/// Перенаправить вывод консоли в консоль.
		/// </summary>
		public TestServerParametersBuilder SetOutputToConsole()
		{
			_serverConfiguration.RedirectConsoleToFileOutput = false;
			return this;
		}

		/// <summary>
		/// Ожидать нажатия клавиши для аттача к серверу в режиме отладки.
		/// </summary>
		public TestServerParametersBuilder SetWaitForAttachDebug()
		{
			_serverConfiguration.WaitForDebugAttach = true;
			return this;
		}

		/// <summary>
		/// Добавить конфигурацию.
		/// </summary>
		public TestServerParametersBuilder AddConfigurationFromAssembly(string assemblyName)
		{
			_serverConfiguration.ConfigurationAssemblies.Add(assemblyName);
			return this;
		}


		/// <summary>
		/// Получить параметры тестового сервераю
		/// </summary>
		public TestServerParameters GetParameters()
		{
			return _serverConfiguration;
		}


		/// <summary>
		/// Установить конфигурацию из JSON конфигурации (в хранилище ElasticSearch) с указанием используемых прикладных сборок.
		/// </summary>
		public TestServerParametersBuilder InstallFromJson(string configurationName, string[] appliedAssemblies)
		{
			InstallJsonConfigurations(configurationName, appliedAssemblies);
			_serverConfiguration.RealConfigNeeds = true;
			return this;
		}

		/// <summary>
		/// Установить конфигурацию из JSON конфигурации (из ZIP файла с архивом конфигурации, без создания конфигурации в ElasticSearch) с указанием используемых прикладных сборок.
		/// </summary>
		public TestServerParametersBuilder InstallFromJsonStub(string configurationName, string[] appliedAssemblies)
		{
			InstallJsonConfigurations(configurationName, appliedAssemblies);
			_serverConfiguration.RealConfigNeeds = false;
			return this;
		}

		private void InstallJsonConfigurations(string configurationFilePath, string[] appliedAssemblies)
		{
			_serverConfiguration.Configurations.Add(new ConfigurationInfo
													{
														AppliedAssemblyList = appliedAssemblies,
														ConfigurationFilePath = configurationFilePath
													});
		}
	}
}