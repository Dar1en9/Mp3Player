using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.UserCommands;

public class FindTracksCommand: ICommand<List<Track>, string>
{
    private readonly IDataBaseReader _dataBaseReader;
    private readonly IHistoryManager _historyManager;
    private readonly ILogger<FindTracksCommand> _logger;
    public string Description => "Найти трек по преподавателю";

    public FindTracksCommand(IDataBaseReader dataBaseReader, IHistoryManager historyManager, 
        ILogger<FindTracksCommand> logger)
    {
        _dataBaseReader = dataBaseReader;
        _historyManager = historyManager;
        _logger = logger;
    }

    public async Task<List<Track>> Execute(string professor)
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        await _historyManager.WriteHistory(professor);
        _logger.LogDebug("История поиска обновлена");
        var tracks = await _dataBaseReader.GetProfessorTracks(professor);
        _logger.LogDebug("Получены треки ({amount}) для преподавателя: {Professor}", tracks.Count, professor);
        return tracks;
    }
}