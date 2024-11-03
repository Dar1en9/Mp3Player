using Mp3Player.Menu.Buttons;

namespace Mp3Player.Menu.Commands;

public class ShowMenuCommand: ICommand<bool, string>
{
    private readonly Dictionary<int, IButton> _buttons;
    public string? Description { get; } = "Доступные команды";

    public ShowMenuCommand(Dictionary<int, IButton> buttons) 
    {
        _buttons = buttons;
    }
    
    Task IUniCommand.Execute()
    {
        return Execute();
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        await Console.Out.WriteLineAsync("Введите номер команды. " + Description + ":");
        foreach (var button in _buttons) 
            await Console.Out.WriteLineAsync($"{button.Key}: {button.Value.Label}"); 
        return true;
    }
}