using System;

namespace Detectors.Redis.Util
{
    public static class StringExtensions
    {
        public static string Truncate(this string s, int length, string indicator = "...")
        {
            if (length <= 0)
                throw new ArgumentException("Length canot be negative or zero");

            if (s == null || s.Length <= length)
                return s;
            
            if (indicator == null)
                indicator = "";
            
            if (length <= indicator.Length)
                throw new ArgumentException("Length should be larger than the length of indicator");

            return s.Substring(0, length - indicator.Length) + indicator;
        }
    }
}