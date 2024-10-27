using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class PlayCommand : ICommand<bool, string>
{
    private readonly Track _track;
    private readonly Player _player;
    public string? Description { get; } = "Воспроизведение";

    public PlayCommand(Track track, Player player)
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
        _player.PlaybackFinished -= OnPlaybackFinished; // Отписка от события
        _player.PlaybackFinished += OnPlaybackFinished; // Подписка на событие
        try
        {
            await _player.Play(_track.audioPath);
            Console.WriteLine("Трек воспроизводится");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return true;
    }

    private static void OnPlaybackFinished(object? sender, EventArgs e)
    {
        Console.WriteLine("Воспроизведение завершено. Вы можете выбрать другой трек или вернуться в меню"); //вернуть
        //на страницу с найденными треками?
    }
}