using System.Collections.Generic;

namespace Root.Controllers.Dto
{
    public class CalculateSumRequest
    {
        public List<CalculateSumRequestItem> Requests { get; set; }
    }
}