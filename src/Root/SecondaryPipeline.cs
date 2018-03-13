using Microsoft.AspNetCore.Http;

namespace Root
{
    public static class SecondaryPipeline
    {
        public static RequestDelegate SecondaryRequestDelegate { get; set; }
    }
}