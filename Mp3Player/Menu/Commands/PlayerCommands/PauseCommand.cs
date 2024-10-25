using Mp3Player.TrackCreator;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class PauseCommand : ICommand<bool, string>
{
    private readonly Track _track;
    private readonly Player _player;
    public string? Description { get; } = "Пауза";

    public PauseCommand(Track track, Player player)
    {
        _track = track;
        _player = player;
    }
    
    Task IUniCommand<string>.Execute(string? arg)
    {
        return Execute(arg);
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        try
        {
            await _player.Pause();
            Console.WriteLine("Трек на паузе");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return true;
    }
}