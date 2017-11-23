using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Hosting layer for request processing based on ASP.NET Core MVC.
    /// </summary>
    public class MvcAppLayer : IBusinessAppLayer, IDefaultAppLayer
    {
        public void Configure(IApplicationBuilder app)
        {
            //app.UseMvc();
        }
    }
}