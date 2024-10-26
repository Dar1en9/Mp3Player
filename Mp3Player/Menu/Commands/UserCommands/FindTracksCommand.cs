using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.TrackCreator;

namespace Mp3Player.Menu.Commands.UserCommands;

public class FindTracksCommand: ICommand<List<Track>, string>
{
    private readonly string _professor;
    private readonly IDataBaseReader _dataBaseReader;
    private readonly IHistoryManager _historyManager;
    public string Description { get; } = "Найти трек по преподавателю";
    
    public FindTracksCommand(string professor, IDataBaseReader dataBaseReader, IHistoryManager historyManager)
    {
        _professor = professor;
        _dataBaseReader = dataBaseReader;
        _historyManager = historyManager;
    }
    
    Task IUniCommand.Execute()
    {
        return Execute();
    }

    public async Task<List<Track>> Execute(string? arg = default)
    {
        await _historyManager.WriteHistory(_professor);   
        return await _dataBaseReader.GetProfessorTracks(_professor);
    }
}