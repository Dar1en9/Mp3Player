using Microsoft.Extensions.Logging;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class ResumeCommand : ICommand<bool, string>
    {
    private readonly Player _player;
    private readonly ILogger _logger;
    public string Description => "Возобновить";

    public ResumeCommand(Player player, ILogger logger)
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
            await _player.Resume();
            _logger.LogInformation("Воспроизведение трека возобновлено");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при возобновлении воспроизведения трека");
            Console.WriteLine(ex.Message);
        }
        return true;
    }
}