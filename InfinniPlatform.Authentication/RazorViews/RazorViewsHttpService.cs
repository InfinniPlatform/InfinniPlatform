using System;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Authentication.RazorViews
{
    public class RazorViewsHttpService : IHttpService
    {
        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "/Razor";
            builder.Get["/View1"] = httpRequest =>
                                    {
                                        var viewHttpResponce = new ViewHttpResponce("AuthenticationView", new DynamicWrapper
                                                                                                          {
                                                                                                              { "Title", "AuthenticationView" },
                                                                                                              { "Data1", Guid.NewGuid() },
                                                                                                              { "Data2", DateTime.Now }
                                                                                                          });

                                        return Task.FromResult<object>(viewHttpResponce);
                                    };

            builder.Get["/View2"] = httpRequest =>
                                    {
                                        var viewHttpResponce = new ViewHttpResponce("BlobStorageView", new DynamicWrapper
                                                                                                          {
                                                                                                              { "Title", "BlobStorageView" },
                                                                                                              { "Data1", Guid.NewGuid() },
                                                                                                              { "Data2", DateTime.Now }
                                                                                                          });

                                        return Task.FromResult<object>(viewHttpResponce);
                                    };
        }
    }
}