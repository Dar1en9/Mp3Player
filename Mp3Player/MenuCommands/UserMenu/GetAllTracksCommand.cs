using Mp3Player.DataBase;
using Mp3Player.TrackCreator;

namespace Mp3Player.MenuCommands.UserMenu;

public class GetAllTracksCommand: ICommand<List<Track>, string>
{
    private readonly IDataBaseReader _dataBaseReader;
    public string? Description { get; } = "Вывести все существующие треки";

    public GetAllTracksCommand(IDataBaseReader dataBaseReader)
    {
        _dataBaseReader = dataBaseReader;
    }

    Task IUniCommand<string>.Execute(string? arg)
    {
        return Execute(arg);
    }
    
    public async Task<List<Track>> Execute(string? arg = default)
    {
        return await _dataBaseReader.ReadAllTracks();
    }
}