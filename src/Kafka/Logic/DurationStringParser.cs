using System;
using System.Text.RegularExpressions;

namespace Detectors.Kafka.Logic
{
    public static class DurationStringParser
    {
        private static readonly Regex DurationExpression = new Regex("^(\\d+)([smh])$");
        
        public static TimeSpan Parse(string durationString)
        {
            if (string.IsNullOrWhiteSpace(durationString))
                return TimeSpan.Zero;

            var match = DurationExpression.Match(durationString);
            int numericValue;
            
            if (match.Success)
            {
                if (!int.TryParse(match.Groups[1].Value, out numericValue))
                    return TimeSpan.Zero;

                switch (match.Groups[2].Value)
                {
                    case "s":
                        return TimeSpan.FromSeconds(numericValue);
                        
                    case "m":
                        return TimeSpan.FromMinutes(numericValue);
                        
                    case "h":
                        return TimeSpan.FromHours(numericValue);
                        
                    default:
                        return TimeSpan.Zero;
                }
            }

            if (int.TryParse(durationString, out numericValue))
                return TimeSpan.FromMinutes(numericValue);
            
            return TimeSpan.Zero;
        }
    }
}