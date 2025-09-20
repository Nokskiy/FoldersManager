namespace FoldersManager;

internal abstract class Program
{
    private static readonly DiskManager DiskManager = new DiskManager();
    private static void Main()
    {
        var pathFinder = new PathFinder(DiskManager);
        var files = pathFinder.ScanDisks("avalonix",@"C:\");
        foreach (var i in files)
            Console.WriteLine(i);
    }
}