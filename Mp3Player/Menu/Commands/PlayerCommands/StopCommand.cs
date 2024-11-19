using Microsoft.Extensions.Logging;
using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class StopCommand : ICommand<bool, string>
{
    private readonly Player _player;
    private readonly ILogger _logger;
    public string Description => "Назад";

    public StopCommand(Player player, ILogger logger)
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
        _logger.LogInformation("Выполнение команды: {Description}", Description);
        try
        {
            await _player.Stop();
            _logger.LogInformation("Воспроизведение трека остановлено");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при остановке воспроизведения трека");
            Console.WriteLine(ex.Message);
        }
        return true;
    }
}