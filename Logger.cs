using System;
using System.IO;

public class Logger
{
    private readonly string _logFilePath;

    public Logger(string logFilePath)
    {
        _logFilePath = logFilePath;

        if (!File.Exists(_logFilePath))
        {
            File.Create(logFilePath).Close();
        }
    }

    public void Log(string message)
    {
        string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
        Console.WriteLine(entry);
        File.AppendAllText(_logFilePath, entry + Environment.NewLine);
    }
}