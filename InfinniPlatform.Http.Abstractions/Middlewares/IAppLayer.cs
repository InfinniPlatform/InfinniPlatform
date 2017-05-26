using Microsoft.AspNetCore.Builder;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Represents application request processing layer.
    /// </summary>
    public interface IAppLayer
    {
        /// <summary>
        /// Configures middlewares of  specific application request processing layer.
        /// </summary>
        /// <param name="app">Application builder.</param>
        void Configure(IApplicationBuilder app);
    }
}