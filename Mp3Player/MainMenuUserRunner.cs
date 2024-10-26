using Mp3Player.Menu.Commands;
using Mp3Player.Menu.Commands.UserCommands;

namespace Mp3Player;

public class MainMenuUserRunner : IRunner
{
    private readonly CommandReader _reader = new CommandReader();
   // public MainMenuUserRunner(Dictionary<int, IUniCommand> commands) => _commands = commands;
    private static readonly Dictionary<int, IUniCommand> _commands = new()
    {
        {1, new FindTracksCommand()} //а как какать?
    };
    
    public Task ExecuteCommand()
    {
        throw new NotImplementedException();
    }

    public Task<IUniCommand> CommandHandler()
    {
        throw new NotImplementedException();
    }

    public Task Run()
    {
        throw new NotImplementedException();
    }
}