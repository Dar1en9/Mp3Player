using Mp3Player.Menu.Commands;

namespace Mp3Player.Menu.UserMenu;

public interface IMenu
{
    Task<IMenu> Run();
    Task ExecuteCommand(IUniCommand command);
    Task<IUniCommand> CommandHandler();
}