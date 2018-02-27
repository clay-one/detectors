using Microsoft.AspNetCore.Mvc;

namespace Detectors.Kafka.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult GetHomepage()
        {
            return Redirect("/api/clusters");
        }
        
        [HttpGet("clusters")]
        public IActionResult GetClusterList()
        {
            return Ok("List of clusters");
        }
    }
}