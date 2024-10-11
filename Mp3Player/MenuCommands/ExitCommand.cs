namespace Mp3Player.MenuCommands;

public class ExitCommand: ICommand<bool, string>
{ 
    public string Description { get; } = "Выйти из приложения";
    
    Task IUniCommand<string>.Execute(string? arg)
    {
        return Execute(arg);
    }
    
    public Task<bool> Execute(string? arg = default)
    {
        Environment.Exit(0);
        return Task.FromResult(false);
    }
}