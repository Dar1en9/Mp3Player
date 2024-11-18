using Mp3Player.Menu.Buttons;

namespace Mp3Player.Menu;

public interface IMenu
{
    Task<IMenu> Run();
    Task ButtonClick(IButton button);
    Task<IButton?> CommandHandler(CancellationToken cancellationToken);
    Task ShowHelp();
}