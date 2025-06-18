This is a console app written in C#.
The program synchronizes two folders: "source" & "replica"

Features:
- One-way synchronization from "source" to "replica
- Automatic periodic syncing (interval defined in seconds)
- Creates new files and folders, updates modified files, and removes  outdated ones
- Logs all operations to console and logs file
- Configurable via command-line arguments

Required:

.NET Framework (https://dotnet.microsoft.com/en-us/download)

To Run:

-Create folders:
1.source
2.replica

Console command:
dotnet run -- "C:\folderpath\source" "C:\folderpath\replica" <syncingperiod_inseconds> "C:\logsfilepath\sync.log"

To stop the application press CTRL+C in console

