using Mp3Player.DataBase;
using Mp3Player.TrackCreator;

namespace Mp3Player.MenuCommands.AdminCommands;

public class AddTrackCommand: ICommand<bool, string>
{
    private readonly Track _track;
    private readonly IDataBaseWriter _dataBaseWriter;
    public string? Description { get; } = "Добавить трек";

    public AddTrackCommand(Track track, IDataBaseWriter dataBaseWriter)
    {
        _track = track;
        _dataBaseWriter = dataBaseWriter;
    }
    
    Task IUniCommand<string>.Execute(string? arg)
    {
        return Execute(arg);
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        await _dataBaseWriter.WriteTrack(_track);
        return true;
    }
}