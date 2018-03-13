using Microsoft.AspNetCore.Mvc;
using Root.Controllers.Dto;

namespace Root.Controllers
{
    public class CalculationController : Controller
    {
        [HttpPost("calculate/sum")]
        public IActionResult CalculateSum(CalculateSumRequest request)
        {
            return Ok();
        }
    }
}