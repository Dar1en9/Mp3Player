using Microsoft.Extensions.Logging;
using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class PauseCommand : ICommand<bool, string>
{
    private readonly Player _player;
    private readonly ILogger _logger;
    public string Description => "Пауза";

    public PauseCommand(Player player, ILogger logger)
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
    
    public async Task<bool> Execute(string? arg = default)
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
            Console.WriteLine(ex.Message);
        }
        return true;
    }
}