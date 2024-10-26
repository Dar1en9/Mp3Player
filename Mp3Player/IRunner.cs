using Mp3Player.Menu.Commands;

namespace Mp3Player;

public interface IRunner
{
    Task ExecuteCommand();
    Task<IUniCommand> CommandHandler();
    Task Run();
}