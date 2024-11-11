using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.AdminCommands;

public class AddTrackCommand: ICommand<bool, string>
{
    private readonly ITrackCreator _trackCreator;
    private readonly IDataBaseWriter _dataBaseWriter;
    public string Description => "Добавить трек";

    public AddTrackCommand(ITrackCreator trackCreator, IDataBaseWriter dataBaseWriter)
    {
        _trackCreator = trackCreator;
        _dataBaseWriter = dataBaseWriter;
    }
    
    Task IUniCommand.Execute()
    {
        return Execute();
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        try
        {
            await _dataBaseWriter.WriteTrack(await _trackCreator.NewTrack());
        }
        catch (MissClickException ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            return false;
        }

        return true;
    }
}