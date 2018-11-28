using System.Collections.Generic;

namespace Root.Controllers.Dto
{
    public class RunBatchRequest
    {
        public List<RunBatchRequestItem> Requests { get; set; }
    }
}