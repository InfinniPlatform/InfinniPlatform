using InfinniPlatform.Core.Http.Services;
using InfinniPlatform.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Nancy.Owin;

namespace InfinniPlatform.Core.Http.Hosting
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                              .AddJsonFile("AppCommon.json")
                              .SetBasePath(env.ContentRootPath);

            Configuration = builder.Build();
        }

        public void Configure(IApplicationBuilder app)
        {
            var config = this.Configuration;
            var appConfig = new AppEnvironment();
            config.Bind(appConfig);

            //TODO Register Nancy bootstrapper.
            //app.UseOwin(x => x.UseNancy(opt => opt.Bootstrapper = new HttpServiceNancyBootstrapper(appConfig)));
        }
    }
}