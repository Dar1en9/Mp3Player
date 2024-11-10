using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.InputReaders;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.AdminCommands;

public class DeleteTrackCommand: ICommand<bool, string>
{
    private readonly IReader<string> _idReader;
    private readonly IDataBaseDeleter _dataBaseDeleter;
    public string? Description { get; } = "Удалить трек";

    public DeleteTrackCommand(IReader<string> idReader, IDataBaseDeleter dataBaseDeleter)
    {
        _idReader = idReader;
        _dataBaseDeleter = dataBaseDeleter;
    }
    
    Task IUniCommand.Execute()
    {
        return Execute();
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        try
        {
            var trackId = await _idReader.GetInput();
            await _dataBaseDeleter.DeleteTrack(trackId);
        }
        catch (MissClickException ex)
        {
            await Console.Out.WriteLineAsync(ex.Message);
            return false;
        }

        return true;
    }
}