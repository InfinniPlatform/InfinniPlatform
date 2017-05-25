using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    public interface IAppLayer
    {
        /// <summary>
        /// Configure application layer.
        /// </summary>
        /// <param name="app">Application builder.</param>
        void Configure(IApplicationBuilder app);
    }
}