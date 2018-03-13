using Microsoft.AspNetCore.Mvc;
using Root.Controllers.Dto;

namespace Root.Controllers
{
    public class BatchController : Controller
    {
        [HttpPost("batch")]
        public IActionResult RunBatch(RunBatchRequest request)
        {
            return Ok();
        }
    }
}