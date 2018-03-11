using System.Collections.Generic;
using System.Linq;
using Detectors.Kafka.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Detectors.Kafka.Controllers
{
    [Route("kafka")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("")]
        public IActionResult GetHomepage()
        {
            return Redirect("/api/kafka/clusters");
        }
        
        [HttpGet("clusters")]
        public IActionResult GetClusterList()
        {
            return Ok(_configuration.GetClusters().Select(c => new {c.Id}));
        }
    }
}