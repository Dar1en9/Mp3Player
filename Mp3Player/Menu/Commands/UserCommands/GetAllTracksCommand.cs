using Mp3Player.DataBase;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.UserCommands;

public class GetAllTracksCommand: ICommand<List<Track>, string>
{
    private readonly IDataBaseReader _dataBaseReader;
    public string Description => "Вывести все существующие треки";

    public GetAllTracksCommand(IDataBaseReader dataBaseReader)
    {
        _dataBaseReader = dataBaseReader;
    }

    Task IUniCommand.Execute()
    {
        return Execute();
    }
    
    public async Task<List<Track>> Execute(string? arg = default)
    {
        return await _dataBaseReader.ReadAllTracks();
    }
}