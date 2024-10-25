using Mp3Player.DataBase;

namespace Mp3Player.Menu.Commands.AdminCommands;

public class DeleteTrackCommand: ICommand<bool, string>
{
    private readonly int _trackId;
    private readonly IDataBaseDeleter _dataBaseDeleter;
    public string? Description { get; } = "Удалить трек";

    public DeleteTrackCommand(int trackId, IDataBaseDeleter dataBaseDeleter)
    {
        _trackId = trackId;
        _dataBaseDeleter = dataBaseDeleter;
    }
    
    Task IUniCommand<string>.Execute(string? arg)
    {
        return Execute(arg);
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        await _dataBaseDeleter.DeleteTrack(_trackId);
        return true;
    }
}