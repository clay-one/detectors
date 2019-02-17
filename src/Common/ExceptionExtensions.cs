using System;
using System.Collections.Generic;

namespace Common
{
    public static class ExceptionExtensions
    {
        public static Dictionary<string, string> GetChainMessageList(this Exception exception)
        {
            var result = new Dictionary<string, string>();

            var index = 0;

            while (exception != null)
            {
                result.Add($"{index} => {exception.GetType().Name}", exception.Message);

                exception = exception.InnerException;
                index++;
            }

            return result;
        }
    }
}
