using Microsoft.Extensions.Logging;
using Mp3Player.Exceptions;
using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class PlayCommand : ICommand<bool, Track>
{
    private readonly Player _player;
    private readonly ILogger _logger;
    public Func<object?, EventArgs, Task>? OnPlaybackFinished { get; set; }
    public string Description => "Воспроизведение";

    public PlayCommand(Player player, ILogger logger)
    {
        _player = player;
        _logger = logger;
    }
    
    Task IUniCommand.Execute()
    {
        _logger.LogWarning("Выполнение команды {Description} было вызвано " +
                           "через универсальный интерфейс IUniCommand", Description);
        return Execute();
    }
    
    public async Task<bool> Execute(Track? track = default)
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        if (OnPlaybackFinished != null)
        {
            _player.PlaybackFinished -= OnPlaybackFinishedWrapper; // Отписка от события
            _player.PlaybackFinished += OnPlaybackFinishedWrapper; // Подписка на событие
            _logger.LogDebug("Подписка на событие по завершению трека");
        }

        if (track == null || string.IsNullOrWhiteSpace(track.AudioPath) || !File.Exists(track.AudioPath))
        {
            _logger.LogWarning("Трек не найден или путь к аудиофайлу пустой");
            throw new NoDataFoundException();
        }
        _logger.LogDebug("Начало воспроизведения трека: {track}", track);
        await _player.Play(track.AudioPath);
        _logger.LogDebug("Завершено воспроизведение трека: {track}", track);
        return true;
    }

    private async void OnPlaybackFinishedWrapper(object? sender, EventArgs e)
    {
        if (OnPlaybackFinished == null) return;
        _logger.LogDebug("Воспроизведение завершено, вызов обработчика события");
        await OnPlaybackFinished(sender, e);
    }
}