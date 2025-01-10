using Microsoft.Extensions.Logging;
using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class PauseCommand : IUniCommand<bool>
{
    private readonly Player _player;
    private readonly ILogger<PauseCommand> _logger;
    public string Description => "Пауза";

    public PauseCommand(Player player, ILogger<PauseCommand> logger)
    {
        _player = player;
        _logger = logger;
    }
    
    public async Task<bool> Execute()
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        try
        {
            await _player.Pause();
            _logger.LogDebug("Воспроизведение трека приостановлено");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при приостановке воспроизведения трека");
            return false;
        }
        return true;
    }
}