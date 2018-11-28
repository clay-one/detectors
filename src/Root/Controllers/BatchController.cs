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
        [HttpPost("batch.{format}")]
        public async Task<IActionResult> RunBatch([FromBody] RunBatchRequest request)
        {
            var tasks = request.Requests.AsParallel().Select(async (r, i) =>
            {
                var result = await SecondaryPipeline.Invoke(r.Uri, requestServices: HttpContext.RequestServices);
                return new RunBatchResponseItem
                {
                    Index = i,
                    StatusCode = result.StatusCode,
                    Body = result.ResponseBody
                };
            });

            var taskResults = await Task.WhenAll(tasks);
            return Ok(new RunBatchResponse {Responses = taskResults.OrderBy(r => r.Index).ToList()});
        }
    }
}