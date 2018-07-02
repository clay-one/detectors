using System;
using System.Collections.Generic;

namespace Detectors.Kafka.Logic
{
    public class RateCalculator
    {
        private const int MinSampleCountToCleanup = 100;
        private const int MinDurationMinutes = 60;
        
        private readonly Queue<RateSample> _samples;
        private readonly object _lockObject;

        public RateCalculator()
        {
            _samples = new Queue<RateSample>();
            _lockObject = new object();
        }

        public long SampleCount
        {
            get
            {
                lock (_lockObject) return _samples.Count;
            }
        }

        public void AddSample(double value)
        {
            lock (_lockObject)
            {
                _samples.Enqueue(new RateSample {Time = DateTime.UtcNow, Value = value});

                if (_samples.Count < MinSampleCountToCleanup)
                    return;

                if (_samples.Count <= 0)
                    return;
                
                var peeked = _samples.Peek();
                if (peeked.Time > (DateTime.UtcNow - TimeSpan.FromMinutes(MinDurationMinutes)))
                    return;

                _samples.Dequeue();
            }
        }

        public double CalculateRateAverage(DateTime from, DateTime to)
        {
            RateSample[] samples;
            
            lock (_lockObject)
            {
                samples = _samples.ToArray();
            }

            if (from >= to)
                return 0;

            if (samples.Length < 2)
                return 0;

            var sampleFrom = CalculateFromSample(samples, from);
            var sampleTo = CalculateToSample(samples, to);

            var deltaTime = (sampleTo.Time - sampleFrom.Time).TotalMinutes;
            var deltaValue = sampleTo.Value - sampleFrom.Value;

            if (deltaTime <= 0)
                return 0;

            return deltaValue / deltaTime;
        }

        private RateSample CalculateFromSample(RateSample[] samples, DateTime time)
        {
            for (var i = 0; i < samples.Length; i++)
            {
                if (samples[i].Time < time)
                    continue;
                
                if (i == 0)
                    return samples[i];

                return Interpolate(samples[i - 1], samples[i], time);
            }

            return samples[samples.Length - 1];
        }

        private RateSample CalculateToSample(RateSample[] samples, DateTime time)
        {
            for (var i = samples.Length - 1; i >= 0; i--)
            {
                if (samples[i].Time > time)
                    continue;
                
                if (i == samples.Length - 1)
                    return samples[i];

                return Interpolate(samples[i], samples[i + 1], time);
            }

            return samples[0];
        }

        private static RateSample Interpolate(RateSample prevSample, RateSample nextSample, DateTime time)
        {
            var d1 = (time - prevSample.Time).TotalMinutes;
            var d2 = (nextSample.Time - time).TotalMinutes;

            if (d1 + d2 <= 0)
                return prevSample;

            var valueDelta = (nextSample.Value - prevSample.Value) / (d1 + d2) * d1;
            return new RateSample {Time = time, Value = prevSample.Value + valueDelta};
        }

    }
}