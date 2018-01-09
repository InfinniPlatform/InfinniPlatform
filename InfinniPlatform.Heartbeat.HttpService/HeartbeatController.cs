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
    public class HeartbeatController : Controller
    {
        /// <summary>
        /// Returns <see cref="OkResult"/>.
        /// </summary>
        /// <param name="id">Identifier.</param>
        [HttpGet("{id}")]
        public object Heartbeat(string id)
        {
            return Ok();
        }
    }
}