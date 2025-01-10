using Microsoft.Extensions.Logging;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class ResumeCommand : IUniCommand<bool>
    {
    private readonly Player _player;
    private readonly ILogger<ResumeCommand> _logger;
    public string Description => "Возобновить";

    public ResumeCommand(Player player, ILogger<ResumeCommand> logger)
    {
        _player = player;
        _logger = logger;
    }
    
    public async Task<bool> Execute()
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        try
        {
            await _player.Resume();
            _logger.LogDebug("Воспроизведение трека возобновлено");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при возобновлении воспроизведения трека");
            return false;
        }
        return true;
    }
}