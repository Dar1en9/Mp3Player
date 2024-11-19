using Microsoft.Extensions.Logging;
using Mp3Player.DataBase;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.UserCommands;

public class GetAllTracksCommand: ICommand<List<Track>, string>
{
    private readonly IDataBaseReader _dataBaseReader;
    private readonly ILogger _logger;
    public string Description => "Вывести все существующие треки";

    public GetAllTracksCommand(IDataBaseReader dataBaseReader, ILogger logger)
    {
        _dataBaseReader = dataBaseReader;
        _logger = logger;
    }

    Task IUniCommand.Execute()
    {
        _logger.LogWarning("Выполнение команды {Description} было вызвано " +
                           "через универсальный интерфейс IUniCommand", Description);
        return Execute();
    }
    
    public async Task<List<Track>> Execute(string? arg = default)
    {
        _logger.LogInformation("Выполнение команды: {Description}", Description);
        var tracks = await _dataBaseReader.ReadAllTracks();
        _logger.LogInformation("Получены все треки ({amount}) из базы данных. Команда {Description} завершила " +
                               "выполнение", tracks.Count, Description);
        return tracks;
    }
}