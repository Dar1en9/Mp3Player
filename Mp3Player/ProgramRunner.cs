using System.Reflection;
using Mp3Player.InputReaders;
using Mp3Player.Menu;

namespace Mp3Player;

public class ProgramRunner(string[] args): IProgramRunner
{
    private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.
        MyMusic), "MP3Player"); //папка MP3Player в папке MyMusic

    public async Task Run()
    {
        if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
        var storagePath = Path.Combine(_path, "Storage");
        if (!Directory.Exists(storagePath)) Directory.CreateDirectory(storagePath);
        if (args.Contains("admin"))
        {
            var menu = new AdminMenus(storagePath);
            await menu.Run();
        }
        else
        {
            var menu = new UserMenus(storagePath, _path);
            await menu.Run();
        }
    }
}