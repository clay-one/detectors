using System.Linq;
using Detectors.MongoDB.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Detectors.MongoDB.Controllers
{
    [Route("mongodb")]
    public class MongoHomeController : Controller
    {
        private readonly MongoClusterConfigCollection _configuration;
        public MongoHomeController(MongoClusterConfigCollection configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("")]
        public IActionResult GetHomepage()
        {
            return Redirect("/api/mongodb/clusters");
        }
        
        [HttpGet("clusters")]
        [HttpGet("clusters.{format}")]
        public IActionResult GetClusterList()
        {
            return Ok(_configuration.GetAllMongoClusterConfigs().Select(c => new {c.Id}).ToList());
        }
    }
}