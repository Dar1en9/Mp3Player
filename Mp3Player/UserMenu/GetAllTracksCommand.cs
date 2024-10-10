using Mp3Player.DataBase;

namespace Mp3Player.UserMenu;

public class GetAllTracksCommand: ICommand<List<Track>, string>
{
    private readonly IDataBaseReader _dataBaseReader;
    public string? Description { get; } = "Вывести все существующие треки";
    
    public GetAllTracksCommand(IDataBaseReader dataBaseReader)
    {
        _dataBaseReader = dataBaseReader;
    }

    public async Task<List<Track>> Execute(string? arg = default)
    {
        return await _dataBaseReader.ReadAllTracks();
    }
}