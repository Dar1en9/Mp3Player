using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class StopCommand : ICommand<bool, string>
{
    private readonly Track _track;
    private readonly Player _player;
    public string? Description { get; } = "Стоп";

    public StopCommand(Track track, Player player)
    {
        _track = track;
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
            Console.WriteLine("Воспроизведение завершено. Вы можете выбрать другой трек или вернуться в меню");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return true;
    }
}