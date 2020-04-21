using System;
using System.Threading;

namespace FieldRules
{
    class Program
    {
        public const string subscriptionKey = "0e4838a555f54d118351f34751a350cd";
        public const string endpoint = @"https://cs-read-pdf-movinova.cognitiveservices.azure.com/";

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
