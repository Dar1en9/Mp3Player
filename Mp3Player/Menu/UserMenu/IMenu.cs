using Mp3Player.Menu.Buttons;
using Mp3Player.Menu.Commands;

namespace Mp3Player.Menu.UserMenu;

public interface IMenu
{
    Task<IMenu> Run();
    Task ButtonClick(IButton button);
    Task<IButton> CommandHandler();
    Task ShowHelp();
}