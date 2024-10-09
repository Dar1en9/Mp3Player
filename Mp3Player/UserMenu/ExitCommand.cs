namespace Mp3Player.UserMenu;

public class ExitCommand: AbstractCommand<bool,string>
{ 
    public string Description { get; } = "Выйти из приложения";

    public Task<bool> Execute(string? arg = default)
    {
        Environment.Exit(0);
        return Task.FromResult(false);
    }
}