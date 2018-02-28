using Microsoft.AspNetCore.Mvc;

namespace Detectors.Kafka.Controllers
{
    public class CalculationController : Controller
    {
        [HttpPost("calculate")]
        public IActionResult DoCalculation()
        {
            return Ok();
        }
    }
}