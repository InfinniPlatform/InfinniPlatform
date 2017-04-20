using System.Threading.Tasks;

using InfinniPlatform.Http;

namespace InfinniPlatform.Heartbeat
{
    /// <summary>
    /// Provides HTTP serivce to get heartbeat status.
    /// </summary>
    /// <example>
    /// <code>
    /// GET /heartbeat
    /// </code>
    /// </example>
    public class HeartbeatHttpService : IHttpService
    {
        public virtual void Load(IHttpServiceBuilder builder)
        {
            builder.Get["/{id}"] = request => Task.FromResult<object>(200);
        }
    }
}