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
        var paths = path == null ? _diskManager.GetDrivesList() : [path];

        foreach (var p in paths)
            result.AddRange(GetAllFilesRecursive(fileName, p));
            
        return result;
    }
    
    private List<string> GetAllFilesRecursive(string fileName,string path)
    {
        List<string> paths = [];

        if (CheckAccess(path))
            paths.AddRange(Directory.EnumerateFiles(path));
        else
            return [];

        var dirs = Directory.EnumerateDirectories(path,"*", _options);
        
        Parallel.ForEach(dirs, dir => paths.AddRange(GetAllFilesRecursive(fileName,dir)));

        return paths.Where(file => Path.GetFileName(file).Contains(fileName,StringComparison)).ToList();
    }

    private bool CheckAccess(string path)
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