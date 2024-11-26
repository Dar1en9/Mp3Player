using Microsoft.Extensions.Logging;
using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.InputReaders;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.UserCommands;

public class FindTracksCommand: ICommand<List<Track>, string>
{
    private readonly IProfessorReader _professorReader;
    private readonly IDataBaseReader _dataBaseReader;
    private readonly IHistoryManager _historyManager;
    private readonly ILogger _logger;
    public string Description => "Найти трек по преподавателю";

    public FindTracksCommand(IProfessorReader professorReader, IDataBaseReader dataBaseReader, 
        IHistoryManager historyManager, ILogger logger)
    {
        _professorReader = professorReader;
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
        var professor = await _professorReader.GetInput();
        _logger.LogInformation("Получено имя преподавателя: {Professor}", professor);
        await _historyManager.WriteHistory(professor);
        _logger.LogDebug("История поиска обновлена");
        var tracks = await _dataBaseReader.GetProfessorTracks(professor);
        _logger.LogDebug("Получены треки ({amount}) для преподавателя: {Professor}", tracks.Count, professor);
        return tracks;
    }
}