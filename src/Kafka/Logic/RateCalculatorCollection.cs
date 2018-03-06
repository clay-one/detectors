using System.Collections.Concurrent;

namespace Detectors.Kafka.Logic
{
    public static class RateCalculatorCollection
    {
        private static readonly object LockObject 
            = new object();

        private static readonly ConcurrentDictionary<string, RateCalculator> Calculators
            = new ConcurrentDictionary<string, RateCalculator>();

        public static RateCalculator GetCalculator(string key, bool createIfNotExists)
        {
            RateCalculator result;
            
            if (createIfNotExists)
                result = Calculators.GetOrAdd(key, _ => new RateCalculator());
            else
                Calculators.TryGetValue(key, out result);
            
            return result;
        }
    }
}