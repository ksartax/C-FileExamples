using System;
using System.IO;
using System.Linq;

namespace CFiles
{
    internal class FileProcessor
    {
        private static readonly string BackupDirectoryName = "backup";
        private static readonly string InProgressDirectoryName = "processing";
        private static readonly string CompleteDirectoryName = "complete";

        public string InputFilePath { get; set; }

        public FileProcessor(string filePath)
        {
            InputFilePath = filePath;
        }

        internal void Process()
        {
            if (!File.Exists(InputFilePath))
            {
                Console.WriteLine($"Plik nie istnieje {InputFilePath}");
                return;
            }

            string rootDirectoryPath = new DirectoryInfo(InputFilePath).Parent.Parent.FullName;
            Console.WriteLine($"root dir {rootDirectoryPath}");

            string DirNamePath = Path.GetDirectoryName(InputFilePath);
            string backupDirPath = Path.Combine(rootDirectoryPath, BackupDirectoryName);

            if (!Directory.Exists(backupDirPath))
            {
                Console.WriteLine($"Create dir {backupDirPath}");
                Directory.CreateDirectory(backupDirPath);
            }

            string inputFileName = Path.GetFileName(InputFilePath);
            string backupFilePath = Path.Combine(backupDirPath, inputFileName);
            Console.WriteLine($"Copy file {inputFileName} into {backupFilePath}");

            File.Copy(InputFilePath, backupFilePath, true);

            string processDirPath = Path.Combine(rootDirectoryPath, InProgressDirectoryName);
            if (!Directory.Exists(processDirPath))
            {
                Directory.CreateDirectory(processDirPath);
            }

            string processFilePath = Path.Combine(processDirPath, inputFileName);
            if (File.Exists(processFilePath))
            {
                Console.WriteLine($"File {processFilePath} already exist in dir {processDirPath}");
                return;
            }

            Console.WriteLine($"Move file {processFilePath} into {processDirPath}");
            File.Move(InputFilePath, processFilePath);

            //Type File
            string fileExtension = Path.GetExtension(InputFilePath);

            string completeDirPath = Path.Combine(rootDirectoryPath, CompleteDirectoryName);
            if (!Directory.Exists(completeDirPath))
            {
                Directory.CreateDirectory(completeDirPath);
            }

            string completedFiledName =
                $"{Path.GetFileNameWithoutExtension(InputFilePath)}--{Guid.NewGuid()}{fileExtension}";
           // completedFiledName = Path.ChangeExtension(completedFiledName, ".complete");
            string completedFilePath = Path.Combine(completeDirPath, completedFiledName);

            switch (fileExtension)
            {
                case ".txt":
                    {
                        var textFileProcessor = new TextFileProcessor(processFilePath, completedFilePath);
                        textFileProcessor.Process();
                    }
                    break;
                case ".data":
                    {
                        var binaryFileProcessor = new BinaryFileProcessor(processFilePath, completedFilePath);
                        binaryFileProcessor.Process();
                    }
                    break;
                case ".csv":
                    {
                        var csvProcessor = new CsvFileProcessor(processFilePath, completedFilePath);
                        csvProcessor.Process();
                    }
                    break;
                default:
                    Console.WriteLine($"Unsuported extension {fileExtension}");
                    break;
            }

            Console.WriteLine($"Delete file : {processFilePath}");
            File.Delete(processFilePath);

            if (Directory.GetFiles(processDirPath).Count() == 0)
            {
                Directory.Delete(processDirPath);
            }
        }
    }
}