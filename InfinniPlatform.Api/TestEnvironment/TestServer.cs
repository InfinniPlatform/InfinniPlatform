using System;
using System.IO;
using System.Runtime.InteropServices;

using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.Settings;

using EnvDTE;

using InfinniPlatform.Api.Threading;

using Newtonsoft.Json;

namespace InfinniPlatform.Api.TestEnvironment
{
	sealed class TestServer : IDisposable
	{
		private volatile TestServerInfo _serverInfo;
		private static readonly object ServerInfoSync = new object();
		private const string TestServerExe = "InfinniPlatform.RestfulApi.exe";
		private static readonly TimeSpan StartTimeout = TimeSpan.FromMinutes(20);


		/// <summary>
		/// Запускает тестовый сервер.
		/// </summary>
		/// <param name="configParameters">Параметры тестового сервера.</param>
		public void Start(Action<TestServerParametersBuilder> configParameters)
		{
			var parametersBuilder = new TestServerParametersBuilder();
			configParameters(parametersBuilder);

			var parameters = parametersBuilder.GetParameters();
			var hostingConfig = parameters.HostingConfig;

			lock (ServerInfoSync)
			{
				// Поиск и остановка существующего сервера
				var existingServer = TestServerTracker.FindTrackedServer(hostingConfig);
				StopExistingTestServer(existingServer);

				// Запуск нового сервера
				_serverInfo = StartNewTestServer(parameters);

				// Ожидание инициализации
				WaitInitTestServer(parameters);

				ControllerRoutingFactory.Instance = new ControllerRoutingFactory(hostingConfig);

#if DEBUG
				if (!parameters.DoNotAttachDebug)
				{
					AttachToTestServer();
				}
#endif
			}
		}

		/// <summary>
		/// Останавливает тестовый сервер.
		/// </summary>
		public void Stop()
		{
			if (_serverInfo != null)
			{
				lock (ServerInfoSync)
				{
					if (_serverInfo != null)
					{
						StopExistingTestServer(_serverInfo);

						_serverInfo = null;
					}
				}
			}
		}

		/// <summary>
		/// Освобождает ресурсы.
		/// </summary>
		public void Dispose()
		{
			Stop();
		}


		private static void StopExistingTestServer(TestServerInfo testServerInfo)
		{
			if (testServerInfo != null)
			{
				try
				{
					testServerInfo.ServerProcess.StopProcess();
				}
				catch
				{
					// Ошибка остановки сервера никому не интересна
				}
				finally
				{
					TestServerTracker.RemoveTrackedServer(testServerInfo);
				}
			}
		}

		private static TestServerInfo StartNewTestServer(TestServerParameters parameters)
		{
			var testServerDir = AppSettings.GetValue("RestServerPath", Directory.GetCurrentDirectory());
			var testServerExe = Path.Combine(testServerDir, TestServerExe);
			var testServerExeArgs = JsonConvert.SerializeObject(parameters).Replace("\"", "'");
			var testServerProcess = new ProcessDispatcher(testServerExe);

			var testServerInfo = new TestServerInfo
								 {
									 ServerProcess = testServerProcess,
									 HostingConfig = parameters.HostingConfig
								 };

			testServerProcess.StartProcess(testServerExeArgs);
			TestServerTracker.AddTrackedServer(testServerInfo);

			return testServerInfo;
		}

		private static void WaitInitTestServer(TestServerParameters parameters)
		{
			var initEventName = string.Format("Api_{0}", parameters.GetServerBaseAddress());

			using (var initEvent = new ProcessEvent(initEventName))
			{
				initEvent.Wait(StartTimeout);
			}
		}

		private static void AttachToTestServer()
		{
			var repeats = 0;

			DTE dte;

			try
			{
				dte = (DTE)Marshal.GetActiveObject("VisualStudio.DTE.11.0");
			}
			catch
			{
				dte = (DTE)Marshal.GetActiveObject("VisualStudio.DTE.12.0");
			}

			while (repeats < 20)
			{
				try
				{
					int procCount = dte.Debugger.DebuggedProcesses.Count;

					if (procCount > 0)
					{
						var processes = dte.Debugger.LocalProcesses;

						foreach (Process proc in processes)
						{
							if (proc.Name.Contains(TestServerExe))
							{
								proc.Attach();
								return;
							}
						}
					}

					return;
				}
				catch
				{
					repeats++;

					Console.WriteLine(Resources.AttemptAttachToTestServer, repeats);
				}
			}

			throw new InvalidOperationException(Resources.CannotAttachToTestServer);
		}
	}
}