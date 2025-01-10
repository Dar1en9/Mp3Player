using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.UserCommands;

public class GetHistoryCommand: IUniCommand<List<Track>>
{
    private readonly IDataBaseReader _dataBaseReader;
    private readonly IHistoryManager _historyManager;
    private readonly ILogger<GetHistoryCommand> _logger;
    public string Description => "Вывести последние треки из истории поиска";

    public GetHistoryCommand(IDataBaseReader dataBaseReader, IHistoryManager historyManager, ILogger<GetHistoryCommand> logger)
    {
        _dataBaseReader = dataBaseReader;
        _historyManager = historyManager;
        _logger = logger;
    }
    public async Task<List<Track>> Execute()
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