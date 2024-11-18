using Mp3Player.Exceptions;
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
        if (track == null || string.IsNullOrWhiteSpace(track.AudioPath) ||
            !File.Exists(track.AudioPath)) throw new NoDataFoundException();
        await _player.Play(track.AudioPath);
        return true;
    }

    private async void OnPlaybackFinishedWrapper(object? sender, EventArgs e)
    {
        await Console.Out.WriteLineAsync("OnPlaybackFinished сработал");
        if (OnPlaybackFinished != null) await OnPlaybackFinished(sender, e); 
    }
}