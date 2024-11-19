using Microsoft.Extensions.Logging;

namespace Mp3Player.Menu.Commands;

public class ExitCommand(ILogger logger): ICommand<bool, string>
{ 
    public string Description => "Выйти из приложения";

    Task IUniCommand.Execute()
    {
        logger.LogWarning("Выполнение команды {Description} было вызвано " +
                          "через универсальный интерфейс IUniCommand", Description);
        return Execute();
    }
    
    public Task<bool> Execute(string? arg = default)
    {
        logger.LogInformation("Выполнение команды: {Description}", Description);
        Environment.Exit(0);
        return Task.FromResult(false);
    }
}