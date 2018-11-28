using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Root.Controllers.Dto;
using Root.Pipeline;

namespace Root.Controllers
{
    public class CalculationController : Controller
    {
        private static readonly Regex NumberExpression = new Regex(@"\d+(\.\d*)?");
        
        [HttpPost("calculate/sum")]
        [HttpPost("calculate/sum.{format}")]
        public async Task<IActionResult> CalculateSum([FromBody] CalculateSumRequest request)
        {
            var tasks = request.Requests.AsParallel().Select(async r =>
            {
                var result = await SecondaryPipeline.Invoke(r.Uri, requestServices: HttpContext.RequestServices);
                if (result.StatusCode != 200)
                    return (double?) null;

                var match = NumberExpression.Match(result.ResponseBody);
                if (!match.Success)
                    return (double?) null;
                
                return double.Parse(match.ToString()) * r.Coefficient;
            });

            var taskResults = await Task.WhenAll(tasks);
            if (taskResults.Any(d => !d.HasValue))
                return BadRequest();
            
            return Ok($"[{taskResults.Sum()}]");
        }
    }
}