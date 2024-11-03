using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.UserCommands;

public class GetHistoryCommand: ICommand<List<Track>, string>
{
    private readonly IDataBaseReader _dataBaseReader;
    private readonly IHistoryManager _historyManager;
    public string Description => "Вывести последние треки из истории поиска";

    public GetHistoryCommand(IDataBaseReader dataBaseReader, IHistoryManager historyManager)
    {
        _dataBaseReader = dataBaseReader;
        _historyManager = historyManager;
    }

    Task IUniCommand.Execute()
    {
        return Execute();
    }
    
    public async Task<List<Track>> Execute(string? arg = default)
    {
        return await _dataBaseReader.GetProfessorTracks(await _historyManager.GetHistory());
    }
}