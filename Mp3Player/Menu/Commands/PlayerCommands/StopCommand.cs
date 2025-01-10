using Microsoft.Extensions.Logging;
using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class StopCommand : IUniCommand<bool>
{
    private readonly Player _player;
    private readonly ILogger<StopCommand> _logger;
    public string Description => "Назад";

    public StopCommand(Player player, ILogger<StopCommand> logger)
    {
        _player = player;
        _logger = logger;
    }
    
    public async Task<bool> Execute()
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        try
        {
            await _player.Stop();
            _logger.LogDebug("Воспроизведение трека остановлено");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при остановке воспроизведения трека");
            return false;
        }
        return true;
    }
}