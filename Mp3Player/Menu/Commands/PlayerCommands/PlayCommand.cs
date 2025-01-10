using Microsoft.Extensions.Logging;
using Mp3Player.Exceptions;
using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class PlayCommand : ICommand<bool, Track>
{
    private readonly Player _player;
    private readonly ILogger<PlayCommand> _logger;
    public string Description => "Воспроизведение";

    public PlayCommand(Player player, ILogger<PlayCommand> logger)
    {
        _player = player;
        _logger = logger;
    }
    
    public async Task<bool> Execute(Track track)
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);

        if (string.IsNullOrWhiteSpace(track.AudioPath) || !File.Exists(track.AudioPath))
        {
            _logger.LogWarning("Трек не найден или путь к аудиофайлу пустой");
            return false;
        }
        _logger.LogDebug("Начало воспроизведения трека: {track}", track);
        await _player.Play(track.AudioPath);
        _logger.LogDebug("Завершено воспроизведение трека: {track}", track);
        return true;
    }
}