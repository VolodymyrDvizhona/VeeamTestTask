using System;
using System.IO;
using System.Collections;

public class FolderSyncService
{
    private readonly string _sourcePath;
    private readonly string _replicaPath;
    private readonly Logger _logger;

    public FolderSyncService(string sourcePath, string replicaPath, Logger logger)
    {
        _sourcePath = sourcePath;
        _replicaPath = replicaPath;
        _logger = logger;
    }

    public void Synchronize()
    {
        if (!Directory.Exists(_sourcePath))
        {
            _logger.Log($"Source path does not exist: {_sourcePath}");
            return;
        }

        if (!Directory.Exists(_replicaPath))
        {
            _logger.Log($"Replica path does not exist – creating: {_replicaPath}");
            Directory.CreateDirectory(_replicaPath);
        }

        CopyFilesRecursive(new DirectoryInfo(_sourcePath), new DirectoryInfo(_replicaPath));
        DeleteRemoved(new DirectoryInfo(_sourcePath), new DirectoryInfo(_replicaPath));
    }

    private bool CompareFiles(string file1, string file2)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        using var stream1 = File.OpenRead(file1);
        using var stream2 = File.OpenRead(file2);
        var hash1 = md5.ComputeHash(stream1);
        var hash2 = md5.ComputeHash(stream2);
        return StructuralComparisons.StructuralEqualityComparer.Equals(hash1, hash2);
    }
    
    private void CopyFilesRecursive(DirectoryInfo source, DirectoryInfo target)
    {
        
        foreach (FileInfo file in source.GetFiles())
        {
            string targetFile = Path.Combine(target.FullName, file.Name);

            if (!File.Exists(targetFile))
            {
                file.CopyTo(targetFile);
                _logger.Log($"Copied file: {file.FullName} -> {targetFile}");
            }
            else if (!CompareFiles(file.FullName, targetFile))
            {
                file.CopyTo(targetFile, true);
                _logger.Log($"Overwritten file (changed): {file.FullName} -> {targetFile}");
            }
        }

        
        foreach (DirectoryInfo subdir in source.GetDirectories())
        {
            DirectoryInfo targetSubDir = target.CreateSubdirectory(subdir.Name);
            CopyFilesRecursive(subdir, targetSubDir);
        }
    }
    
    private void DeleteRemoved(DirectoryInfo source, DirectoryInfo target)
    {
      
        foreach (FileInfo file in target.GetFiles())
        {
            string sourceFile = Path.Combine(source.FullName, file.Name);
            if (!File.Exists(sourceFile))
            {
                file.Delete();
                _logger.Log($"Deleted file (not in source): {file.FullName}");
            }
        }

     
        foreach (DirectoryInfo dir in target.GetDirectories())
        {
            string sourceDir = Path.Combine(source.FullName, dir.Name);
            if (!Directory.Exists(sourceDir))
            {
                dir.Delete(true);
                _logger.Log($"Deleted folder (not in source): {dir.FullName}");
            }
            else
            {
                DeleteRemoved(new DirectoryInfo(sourceDir), dir);
            }
        }
    }

    
}