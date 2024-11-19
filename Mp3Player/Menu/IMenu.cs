using Mp3Player.Menu.Buttons;

namespace Mp3Player.Menu;

public interface IMenu
{
    public string Label { get; }
    Task<IMenu> Run();
    Task ButtonClick(IButton button);
    Task<IButton?> CommandHandler(CancellationToken cancellationToken);
    Task ShowHelp();
}