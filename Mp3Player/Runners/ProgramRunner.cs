using Microsoft.Extensions.Logging;
using Mp3Player.Menu;
using Mp3Player.Menu.Pages;

namespace Mp3Player.Runners;

public class ProgramRunner(string[] args, ILogger<ProgramRunner> logger): IProgramRunner
{
    private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.
        MyMusic), "MP3Player"); //папка MP3Player в папке MyMusic

    public async Task Run()
    {
        logger.LogDebug("Запуск программы");
        if (!Directory.Exists(_path))
        {
            logger.LogDebug("Создание директории: {Path}", _path);
            Directory.CreateDirectory(_path);
        }
        var storagePath = Path.Combine(_path, "Storage");
        if (!Directory.Exists(storagePath))
        {
            logger.LogDebug("Создание директории: {StoragePath}", storagePath);
            Directory.CreateDirectory(storagePath);
        }

        if (args.Contains("admin"))
        {
            logger.LogDebug("Запуск AdminPages");
            await new AdminPages(storagePath, logger).Run();
        }
        else
        {
            logger.LogDebug("Запуск UserPages");
            await new UserPages(storagePath, _path, logger).Run();
        }
    }
}