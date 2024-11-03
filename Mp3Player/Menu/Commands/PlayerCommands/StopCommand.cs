using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class StopCommand : ICommand<bool, string>
{
    private readonly Player _player;
    public string Description => "Стоп";

    public StopCommand(Player player)
    {
        _player = player;
    }
    
    Task IUniCommand.Execute()
    {
        return Execute();
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        try
        {
            await _player.Stop();
            Console.WriteLine("Воспроизведение завершено. Возвращение к списку треков");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return true;
    }
}