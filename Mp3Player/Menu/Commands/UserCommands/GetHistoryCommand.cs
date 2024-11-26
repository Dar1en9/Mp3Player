using Microsoft.Extensions.Logging;
using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.UserCommands;

public class GetHistoryCommand: ICommand<List<Track>, string>
{
    private readonly IDataBaseReader _dataBaseReader;
    private readonly IHistoryManager _historyManager;
    private readonly ILogger _logger;
    public string Description => "Вывести последние треки из истории поиска";

    public GetHistoryCommand(IDataBaseReader dataBaseReader, IHistoryManager historyManager, ILogger logger)
    {
        _dataBaseReader = dataBaseReader;
        _historyManager = historyManager;
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
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        var history = await _historyManager.GetHistory();
        _logger.LogDebug("Получена история поиска");
        var tracks = await _dataBaseReader.GetProfessorTracks(history);
        _logger.LogDebug("Получены треки из базы данных. Команда {Description} завершила " +
                         "выполнение", Description);
        return tracks;
    }
}