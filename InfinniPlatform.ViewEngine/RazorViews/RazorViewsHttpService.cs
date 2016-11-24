using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.ViewEngine.RazorViews
{
    /// <summary>
    /// Пример сервиса, возвращающего Razor-пресдтавления.
    /// </summary>
    /// <remarks>
    /// Подробнее http://infinniplatform.readthedocs.io/ru/latest/14-view-engine/index.html.
    /// </remarks>
    public class RazorViewsHttpService : IHttpService
    {
        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/razor";
            // Возвращаем Razor-представление Index.cshtml, передавая динамическую модель данных.
            builder.Get["/View1"] = httpRequest =>
                                    {
                                        var viewHttpResponce = new ViewHttpResponce("View1", new DynamicWrapper
                                                                                             {
                                                                                                 { "Title", "Title" },
                                                                                                 { "Data1", "Somedata" },
                                                                                                 { "Data2", DateTime.Now }
                                                                                             });

                                        return Task.FromResult<object>(viewHttpResponce);
                                    };

            // Возвращаем Razor-представление About.cshtml, не принимающее модель данных.
            builder.Get["/View2"] = httpRequest1 => Task.FromResult<object>(new ViewHttpResponce("View2"));
        }
    }
}