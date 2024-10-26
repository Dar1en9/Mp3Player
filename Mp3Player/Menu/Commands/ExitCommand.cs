namespace Mp3Player.Menu.Commands;

public class ExitCommand: ICommand<bool, string>
{ 
    public string Description { get; } = "Выйти из приложения";
    
    Task IUniCommand.Execute()
    {
        return Execute();
    }
    
    public Task<bool> Execute(string? arg = default)
    {
        Environment.Exit(0);
        return Task.FromResult(false);
    }
}