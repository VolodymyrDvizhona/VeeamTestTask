using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 4)
        {
            Console.WriteLine("Usage: VeeamTestTask.exe <sourcePath> <replicaPath> <syncIntervalSec> <logFilePath>");
            return;
        }

        string sourcePath = args[0];
        string replicaPath = args[1];
        string intervalArg = args[2];
        string logPath = args[3];

        if (!int.TryParse(intervalArg, out int intervalSec))
        {
            Console.WriteLine("Invalid synchronization interval (must be an integer).");
            return;
        }

        Logger logger = new Logger(logPath);
        FolderSyncService syncService = new FolderSyncService(sourcePath, replicaPath, logger);

        logger.Log("Synchronization started.");

        while (true)
        {
            logger.Log("Starting new sync cycle...");
            syncService.Synchronize();
            logger.Log($"Waiting {intervalSec} seconds until next cycle...");
            Thread.Sleep(intervalSec * 1000);
        }
    }
}
