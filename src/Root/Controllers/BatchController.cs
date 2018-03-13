using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Root.Controllers.Dto;
using Root.Pipeline;

namespace Root.Controllers
{
    public class BatchController : Controller
    {
        [HttpPost("batch")]
        public async Task<IActionResult> RunBatch([FromBody] RunBatchRequest request)
        {
            var tasks = request.Requests.AsParallel().Select(async r =>
            {
                var result = await SecondaryPipeline.Invoke(r.Uri, requestServices: HttpContext.RequestServices);
                return new RunBatchResponseItem
                {
                    StatusCode = result.StatusCode,
                    Body = result.ResponseBody
                };
            });

            var taskResults = await Task.WhenAll(tasks);
            return Ok(new RunBatchResponse {Responses = taskResults.ToList()});
        }
    }
}