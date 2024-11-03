using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class PlayCommand : ICommand<bool, Track>
{
    private readonly Player _player;
    public Func<object?, EventArgs, Task>? OnPlaybackFinished { get; set; }
    public string Description => "Воспроизведение";

    public PlayCommand(Player player)
    {
        _player = player;
    }
    
    Task IUniCommand.Execute()
    {
        return Execute();
    }
    
    public async Task<bool> Execute(Track? track = default)
    {
        if (OnPlaybackFinished != null)
        {
            _player.PlaybackFinished -= OnPlaybackFinishedWrapper; // Отписка от события
            _player.PlaybackFinished += OnPlaybackFinishedWrapper; // Подписка на событие
        }

        try
        {
            await _player.Play(track!.AudioPath);
            Console.WriteLine("Трек воспроизводится");
        }
        catch (Exception ex)
        {
            //сделать логи
            Console.WriteLine(ex.Message);
        }
        return true;
    }

    private async void OnPlaybackFinishedWrapper(object? sender, EventArgs e)
    {
        if (OnPlaybackFinished != null) await OnPlaybackFinished(sender, e); 
    }
}