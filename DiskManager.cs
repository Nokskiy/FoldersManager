namespace FoldersManager;

public class DiskManager
{
    public List<string> GetDrivesList()
    {
        var result = DriveInfo.GetDrives().ToList().ConvertAll(diks => diks.ToString());
        return result;
    }
}