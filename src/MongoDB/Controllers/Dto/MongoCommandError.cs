namespace Detectors.MongoDB.Controllers.Dto
{
    public class MongoCommandError
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public string CodeName { get; set; }
    }
}
