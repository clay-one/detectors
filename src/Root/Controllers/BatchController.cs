using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Root.Controllers.Dto;

namespace Root.Controllers
{
    public class BatchController : Controller
    {
        [HttpPost("batch")]
        public async Task<IActionResult> RunBatch([FromBody] RunBatchRequest request)
        {
            var tasks = request.Requests.AsParallel().Select(async r =>
            {
                var responseStream = new MemoryStream();

                HttpContext innerContext = new DefaultHttpContext();
                innerContext.Request.Method = "GET";
                innerContext.Request.Path = r.Uri;
                innerContext.RequestServices = HttpContext.RequestServices;
                innerContext.Response.Body = responseStream;

                await SecondaryPipeline.SecondaryRequestDelegate(innerContext);

                return new RunBatchResponseItem
                {
                    StatusCode = innerContext.Response.StatusCode,
                    Body = Encoding.UTF8.GetString(responseStream.ToArray())
                };
            });

            var taskResults = await Task.WhenAll(tasks);
            return Ok(new RunBatchResponse {Responses = taskResults.ToList()});
        }
    }
}