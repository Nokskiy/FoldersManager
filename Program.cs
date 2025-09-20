namespace FoldersManager;

internal abstract class Program
{
    private static readonly DiskManager DiskManager = new DiskManager();

    private static void Main(string[] args)
    {
        var pathFinder = new PathFinder(DiskManager);

        if (args.Length < 1)
        {
            Console.WriteLine("Arguments is null");
            return;
        }

        var files = pathFinder.ScanDisks(args[0], args.Length > 1 ? args[1] : null);

        if (files.Count == 0)
        {
            Console.WriteLine("No files found.");
            return;
        }

        foreach (var i in files)
            Console.WriteLine(i);
        
        Console.ReadLine();
    }
}