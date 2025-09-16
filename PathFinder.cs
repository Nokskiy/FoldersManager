namespace FoldersManager;

public class PathFinder
{
    private readonly EnumerationOptions _options = new EnumerationOptions
    {
        IgnoreInaccessible = true,
        AttributesToSkip = FileAttributes.System,
        RecurseSubdirectories = false,
        MatchType = MatchType.Win32
    };

    private const StringComparison StringComparison = System.StringComparison.OrdinalIgnoreCase;

    public List<string> GetAllFilesRecursive(string partOfFileName,string path)
    {
        List<string> paths = [];

        if (CheckAccess(path))
            paths.AddRange(Directory.EnumerateFiles(path));
        else
            return [];

        var dirs = Directory.EnumerateDirectories(path,"*", _options);
        
        Parallel.ForEach(dirs, dir =>
        {
            paths.AddRange(GetAllFilesRecursive(partOfFileName,dir));
        });

        return paths.Where(file => file.Contains(partOfFileName,StringComparison)).ToList();
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