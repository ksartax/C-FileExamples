using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CFiles
{
    public class BinaryFileProcessor
    {
        public string InputFilePath { get; set; }
        public string OutputFilePath { get; set; }

        public BinaryFileProcessor(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
        }

        public void Process()
        {
            //byte[] data = File.ReadAllBytes(InputFilePath);
            //byte largest = data.Max();
            //byte[] newData = new byte[data.Length + 1];
            //Array.Copy(newData, data, data.Length);
            //newData[newData.Length - 1] = largest;

            //File.WriteAllBytes(OutputFilePath, newData);

            //using (FileStream input = File.Open(InputFilePath, FileMode.Open, FileAccess.Read))
            //using (FileStream output = File.Create(OutputFilePath))
            //{
            //    const int endOfStream = -1;
            //    int largestByte = 0;

            //    int currentByte = input.ReadByte();

            //    while (currentByte != endOfStream)
            //    {
            //        output.WriteByte((byte) currentByte);

            //        if (currentByte > largestByte)
            //        {
            //            largestByte = currentByte;
            //        }

            //        currentByte = input.ReadByte();
            //    }

            //    output.WriteByte((byte) largestByte);
            //}

            using (FileStream inputFileStream = File.Open(InputFilePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryStreamReader = new BinaryReader(inputFileStream))
            using (FileStream outputFileStream = File.Create(OutputFilePath))
            using (BinaryWriter binaryStreamWriter = new BinaryWriter(outputFileStream))
            {
                byte large = 0;
                while (binaryStreamReader.BaseStream.Position > binaryStreamReader.BaseStream.Length)
                {
                    byte currentByte = binaryStreamReader.ReadByte();

                    binaryStreamWriter.Write(currentByte);

                    if (currentByte > large)
                    {
                        large = currentByte;
                    }
                }

                binaryStreamWriter.Write(large);
            }
        }
    }
}
