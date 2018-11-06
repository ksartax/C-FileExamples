using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CFiles
{
    internal class CsvFileProcessor
    {
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }

        public CsvFileProcessor(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
        }

        public void Process()
        {
            using (StreamReader input = File.OpenText(InputFilePath))
            using (CsvReader csvReader = new CsvReader(input))
            using (StreamWriter output = File.CreateText(OutputFilePath))
            using (CsvWriter csvWriter = new CsvWriter(output))
            {
                csvReader.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
                csvReader.Configuration.Comment = '@'; //default #
                csvReader.Configuration.AllowComments = true;
                csvReader.Configuration.IgnoreBlankLines = true;
                //csvReader.Configuration.HeaderValidated = null;
                //csvReader.Configuration.MissingFieldFound = null;
                //csvReader.Configuration.Delimiter = ":";
                //csvReader.Configuration.HasHeaderRecord = false;

                csvReader.Configuration.RegisterClassMap<ProcessOrderMap>();
                IEnumerable<ProcessOrder> records = csvReader.GetRecords<ProcessOrder>();

                //IEnumerable<dynamic> records = csvReader.GetRecords<dynamic>();
                //IEnumerable<Order> records = csvReader.GetRecords<Order>();

                foreach (var record in records)
                {
                    //System.Console.WriteLine(record.Field1);
                    System.Console.WriteLine(record.OrderNumber);
                    System.Console.WriteLine(record.Customer);
                    System.Console.WriteLine(record.Amount);
                    //System.Console.WriteLine(record.CustomerNumber);
                    //System.Console.WriteLine(record.Description);
                    //System.Console.WriteLine(record.Quantity);
                }

                csvWriter.WriteHeader<ProcessOrder>();
                csvWriter.NextRecord();

                var recordsArray = records.ToArray();
                for (int i = 0; i < recordsArray.Length; i++)
                {
                    csvWriter.WriteField(recordsArray[i].OrderNumber);
                    csvWriter.WriteField(recordsArray[i].Customer);
                    csvWriter.WriteField(recordsArray[i].Amount);

                    bool isLastRecord = i == recordsArray.Length - 1;

                    if (isLastRecord)
                    {
                        csvWriter.NextRecord();
                    }
                }
            }
        }
    }
}