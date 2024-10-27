using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.InputReaders;
using Mp3Player.TrackCreator;

namespace Mp3Player.Menu.Commands.UserCommands;

public class FindTracksCommand: ICommand<List<Track>, string>
{
    private readonly ProfessorReader _professorReader;
    private readonly IDataBaseReader _dataBaseReader;
    private readonly IHistoryManager _historyManager;
    public string Description { get; } = "Найти трек по преподавателю";
    
    public FindTracksCommand(ProfessorReader professorReader, IDataBaseReader dataBaseReader, IHistoryManager historyManager)
    {
        _professorReader = professorReader;
        _dataBaseReader = dataBaseReader;
        _historyManager = historyManager;
    }
    
    Task IUniCommand.Execute()
    {
        return Execute();
    }

    public async Task<List<Track>> Execute(string? arg = default)
    {
        var professor = await _professorReader.GetInput();
        await _historyManager.WriteHistory(professor);   
        return await _dataBaseReader.GetProfessorTracks(professor);
    }
}