using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FieldRules
{
    public class BatchReadFile
    {
        string NITValue = string.Empty;
        string IVAValue = string.Empty;
        string InvoiceNumber = string.Empty;
        string Subtotal = string.Empty;
        string InvoiceDate = string.Empty;

        public async Task RunAsync(string endpoint, string key)
        {
            ComputerVisionClient computerVision = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };

            const int numberOfCharsInOperationId = 36;
            string localImagePath = @"D:\TEMP\OCR\1.pdf";
            BatchReadFileFromStreamAsync(computerVision, localImagePath, numberOfCharsInOperationId).Wait(5000);
        }

        private async Task BatchReadFileFromUrlAsync(ComputerVisionClient computerVision, string imageUrl, int numberOfCharsInOperationId)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Console.WriteLine("\nInvalid remote image url:\n{0} \n", imageUrl);
                return;
            }

            // Start the async process to read the text
            BatchReadFileHeaders textHeaders = computerVision.BatchReadFileAsync(imageUrl).Result;
            await GetTextAsync(computerVision, textHeaders.OperationLocation, numberOfCharsInOperationId);
        }

        private async Task BatchReadFileFromStreamAsync(ComputerVisionClient computerVision, string imagePath, int numberOfCharsInOperationId)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine("\nUnable to open or read local image path:\n{0} \n", imagePath);
                return;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                BatchReadFileInStreamHeaders textHeaders = await computerVision.BatchReadFileInStreamAsync(imageStream);
                await GetTextAsync(computerVision, textHeaders.OperationLocation, numberOfCharsInOperationId);
            }
        }

        private async Task GetTextAsync(ComputerVisionClient computerVision, string operationLocation, int numberOfCharsInOperationId)
        {
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);
            ReadOperationResult result = await computerVision.GetReadOperationResultAsync(operationId);

            // Wait for the operation to complete
            int i = 0;
            int maxRetries = 10;

            while ((result.Status == TextOperationStatusCodes.Running ||
                    result.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries)
            {
                Console.WriteLine("Server status: {0}, waiting {1} seconds...", result.Status, i);
                await Task.Delay(1000);
                result = await computerVision.GetReadOperationResultAsync(operationId);
            }

            // Display the results
            var recResults = result.RecognitionResults;

            foreach (TextRecognitionResult recResult in recResults)
            {
                foreach (Line line in recResult.Lines)
                {
                    ProcessValue(line.Text);
                }
            }

            Console.WriteLine($"NIT:{NITValue}");
            Console.WriteLine($"NIT:{IVAValue}");
            Console.WriteLine($"NIT:{InvoiceNumber}");
            Console.WriteLine($"NIT:{Subtotal}");
            Console.WriteLine($"NIT:{InvoiceDate}");

            Console.ReadLine();
        }

        private void ProcessValue(string value)
        {
            //Date might be needed to convert to DateTime
            string Date = string.Empty;

            //Validate NIT Rules
            if (NITRules.Validate(value))
            {
                NITValue = NITRules.ExtractNit(value);
            }

            //Validate IVA Rules
            if (IVARules.Validate(value))
            {
                IVAValue = IVARules.ExtractIVA(value);
            }

            //Validate Invoice Number
            if (InvoiceNumberRules.Validate(value))
            {
                InvoiceNumber = InvoiceNumberRules.ExtractInvoiceNumber(value);
            }

            //Validate Subtotal
            if (SubtotalRules.Validate(value))
            {
                Subtotal = SubtotalRules.ExtractSubtotal(value);
            }

            //Validate Subtotal
            if (InvoiceDateRules.Validate(value))
            {
                string tempDate = InvoiceDateRules.ExtractDate(value);
                
                //Is Invoice Date if the current year matches with invoice year
                InvoiceDate = (InvoiceDateRules.IsCurrentYear(tempDate)) ? tempDate : string.Empty;
            }
        }
    }
}
