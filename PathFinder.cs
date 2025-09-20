namespace FoldersManager;

public class PathFinder(DiskManager diskManager)
{
    private readonly DiskManager _diskManager = new DiskManager();
    private readonly EnumerationOptions _options = new EnumerationOptions
    {
        IgnoreInaccessible = true,
        AttributesToSkip = FileAttributes.System,
        RecurseSubdirectories = false,
        MatchType = MatchType.Win32
    };

    private const StringComparison StringComparison = System.StringComparison.OrdinalIgnoreCase;

    public List<string> ScanDisks(string fileName,string? path)
    {
        List<string> result = [];

        if (path != null && !Path.Exists(path))
        {
            Console.WriteLine("Please enter the correct start path.\nExample: \"C:\\\"");
            Console.ReadLine();
            Environment.Exit(1);
        }

        Parallel.ForEach(path == null ? _diskManager.GetDrivesList() : [path],
            p =>result.AddRange(GetAllFilesRecursive(fileName, p)));
        
        return result;
    }
    
    private List<string> GetAllFilesRecursive(string fileName,string path)
    {
        List<string> paths = [];

        if (CheckAccess(path))
            paths.AddRange(Directory.EnumerateFiles(path,$"*{fileName}*"));
        else
            return [];
        
        Parallel.ForEach(Directory.EnumerateDirectories(path,"*", _options), dir => paths.AddRange(GetAllFilesRecursive(fileName,dir)));

        return paths;
    }

    private static bool CheckAccess(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.GetFiles(path);
                Directory.GetDirectories(path);
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
}