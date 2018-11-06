using System;
using System.IO;
using System.Runtime.Caching;

namespace CFiles
{
    class Program
    {
        private static readonly MemoryCache FilesToProcess = MemoryCache.Default; 

        static void Main(string[] args)
        {
            var element = args[0];

            switch (element)
            {
                case "--file":
                    {
                        var filePath = args[1];
                        Console.WriteLine($"file path {filePath}");
                        ProcessSingleFile(filePath);
                    }
                    break;
                case "--dir":
                    {
                        var dirPath = args[1];
                        var filePath = args[2];
                        Console.WriteLine($"Dir path {dirPath} | file path {filePath}");
                        ProcessDirectory(dirPath, filePath);
                    }
                    break;
                case "--watch":
                    {
                        var dirPath = args[1];
                        WatchDir(dirPath);
                    }
                    break;
                default:
                    break;
            }

            Console.ReadLine();
        }

        private static void WatchDir(string dirPath)
        {
            Console.WriteLine($"dir {dirPath} is start watching");

            using (var inputFilterWatcher = new FileSystemWatcher(dirPath))
            {
                inputFilterWatcher.IncludeSubdirectories = false;
                inputFilterWatcher.Filter = "*.*";
                inputFilterWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

                inputFilterWatcher.Created += FileCreated;
                inputFilterWatcher.Changed += FileChanged;
                inputFilterWatcher.Deleted += FileDeleted;
                inputFilterWatcher.Renamed += FileRenamed;
                inputFilterWatcher.Error += FileError;

                inputFilterWatcher.EnableRaisingEvents = true;

                Console.ReadLine();
            }
        }

        private static void FileError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"File error {e.GetException()}");
        }

        private static void FileRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"File renamed: {e.Name} - type: {e.ChangeType}");
        }

        private static void FileDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File deleted: {e.Name} - type: {e.ChangeType}");
        }

        private static void FileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File changed: {e.Name} - type: {e.ChangeType}");
        }

        private static void FileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"File created: {e.Name} - type: {e.ChangeType}");
            AddToCache(e.FullPath);
        }

        private static void AddToCache(string fullPath)
        {
            var item = new CacheItem(fullPath, fullPath);

            var policy = new CacheItemPolicy
            {
                RemovedCallback = ProcessFile,
                SlidingExpiration = TimeSpan.FromSeconds(2)
            };

            FilesToProcess.Add(item, policy);
        }

        private static void ProcessFile(CacheEntryRemovedArguments arguments)
        {
            Console.WriteLine($"Cache item removed : {arguments.CacheItem.Key}, become {arguments.RemovedReason}");

            if (arguments.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                var fileProcessor = new FileProcessor(arguments.CacheItem.Key);
                fileProcessor.Process();
            }
            else
            {
                Console.WriteLine($"Warning, {arguments.CacheItem.Key} was removed unexpectable");
            }
        }

        private static void ProcessDirectory(string dirPath, string filePath)
        {
            switch (filePath)
            {
                case "TXT":
                    {
                        string[] textFiles = Directory.GetFiles(dirPath, "*.txt");
                        foreach (var textFilePath in Directory.GetFiles(dirPath))
                        {
                            var fileProcessor = new FileProcessor(textFilePath);
                            fileProcessor.Process();
                        }
                    }
                    break;
                default:
                    Console.WriteLine();
                    break;
            }
        }

        private static void ProcessSingleFile(string filePath)
        {
            FileProcessor fileProcessor = new FileProcessor(filePath);
            fileProcessor.Process();
        }
    }
}
