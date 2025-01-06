using Dapper;
using Microsoft.Extensions.Logging;
using Mp3Player.Menu.Pages;
using Mp3Player.TrackHandler;
using Npgsql;

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
        var connectionString = ConfigBuilder.AppConfigSettings.DefaultConnection; 
        logger.LogDebug("Подключение к базе данных: {DefaultConnection}", connectionString);
        var connection = new NpgsqlConnection(connectionString);
        if (args.Contains("admin"))
        {
            logger.LogDebug("Запуск AdminPages");
            await new AdminPages(connection, logger).Run();
        }
        else
        {
            logger.LogDebug("Запуск UserPages");
            await new UserPages(connection, _path, logger).Run();
        }
    }
}