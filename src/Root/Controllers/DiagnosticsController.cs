using Microsoft.AspNetCore.Mvc;

namespace Root.Controllers
{
    [Route("diagnostics")]
    public class DiagnosticsController : Controller
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}