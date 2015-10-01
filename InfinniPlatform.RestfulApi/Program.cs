using System;
using System.Linq;
using System.Threading;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.Api.Threading;
using InfinniPlatform.Logging;
using InfinniPlatform.RestfulApi.Properties;
using InfinniPlatform.WebApi.Logging;
using Newtonsoft.Json;

namespace InfinniPlatform.RestfulApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetupLogErrorHandler();

            try
            {
                // В качестве входного аргумента ожидается сериализованный JSON-объект типа TestServerParameters
                TestServerParameters parameters = args.Any()
                                                      ? JsonConvert.DeserializeObject<TestServerParameters>(args[0])
                                                      : new TestServerParameters {RealConfigNeeds = true};

                string initEventName = string.Format("Api_{0}", parameters.GetServerBaseAddress());

                using (var initEvent = new ProcessEvent(initEventName))
                {
                    try
                    {
                        var console = new ApiConsole();
                        console.Run(parameters);
                    }
                    finally
                    {
                        // В любом случае уведомляем ждущие процессы
                        initEvent.Set();
                    }
                }
            }
            catch (Exception error)
            {
                // Выводим ошибку и не пробрасываем ее дальше
                Logger.Log.Error(Resources.CannotStartTestServer, error);
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}