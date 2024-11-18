using Mp3Player.Menu;

namespace Mp3Player.Runners;

public class ProgramRunner(string[] args): IProgramRunner
{
    private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.
        MyMusic), "MP3Player"); //папка MP3Player в папке MyMusic

    public async Task Run()
    {
        if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
        var storagePath = Path.Combine(_path, "Storage");
        if (!Directory.Exists(storagePath)) Directory.CreateDirectory(storagePath);
        if (args.Contains("admin")) await new AdminMenu(storagePath).Run();
        else await new UserMenu(storagePath, _path).Run();
    }
}