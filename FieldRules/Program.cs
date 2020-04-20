using System;
using System.Threading;

namespace FieldRules
{
    class Program
    {
        public const string subscriptionKey = "";
        public const string endpoint = @"";

        static void Main(string[] args)
        {
            try
            {
                BatchReadFile batchReadFile = new BatchReadFile();

                while (true)
                {
                    var myString = batchReadFile.RunAsync(endpoint, subscriptionKey).Wait(50000000);
                    Thread.Sleep(100000000);
                }

                //batchReadFile.RunAsync(endpoint, subscriptionKey).Wait(50000000);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
