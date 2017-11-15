using Microsoft.AspNetCore.Mvc;

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
    [Route("heartbeat")]
    public class HeartbeatHttpService : Controller
    {
        [HttpGet("{id}")]
        public object Heartbeat(string id)
        {
            return Ok();
        }
    }
}