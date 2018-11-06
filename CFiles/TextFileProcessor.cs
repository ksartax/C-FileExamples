using System.IO;

namespace CFiles
{
    public class TextFileProcessor
    {
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }

        public TextFileProcessor(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
        }

        public void Process()
        {
            //using (var inputFileStream = new FileStream(InputFilePath, FileMode.Open))
            //{
            //    using (var inputStreamReader = new StreamReader(inputFileStream))
            //    {
            //        using (var outputFileStream = new FileStream(OutputFilePath, FileMode.Create))
            //        {
            //            using (var outputStremWriter = new StreamWriter(outputFileStream))
            //            {
            //                while (!inputStreamReader.EndOfStream)
            //                {
            //                    string line = inputStreamReader.ReadLine();
            //                    line = line.ToUpperInvariant();
            //                    outputStremWriter.WriteLine(line);
            //                }
            //            }
            //        }
            //    }
            //} 

            using (var inputStreamReader = new StreamReader(InputFilePath))
            using (var outputStreamReader = new StreamWriter(OutputFilePath))
            {
                while (!inputStreamReader.EndOfStream)
                {
                    string line = inputStreamReader.ReadLine();
                    line = line.ToUpperInvariant();

                    if (inputStreamReader.EndOfStream)
                    {
                        outputStreamReader.Write(line);
                    }
                    else
                    {
                        outputStreamReader.WriteLine(line);
                    }
                }
            }

            //string originalText = File.ReadAllText(InputFilePath);
            //string processedText = originalText.ToUpperInvariant();
            //File.WriteAllText(InputFilePath, processedText);

            //string[] lines = File.ReadAllLines(InputFilePath);
            //lines[1] = lines[1].ToUpperInvariant();
            //File.WriteAllLines(OutputFilePath, lines);
        }
    }
}
