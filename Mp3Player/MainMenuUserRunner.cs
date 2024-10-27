using Mp3Player.InputReaders;
using Mp3Player.Menu.Commands;
using Mp3Player.Menu.Commands.UserCommands;

namespace Mp3Player;

public class MainMenuUserRunner : IRunner
{
    private readonly CommandReader _reader = new CommandReader(); //должна быть общая start страница где всё это
    //создается, потому что мы будем запускать главное меню не один раз
   // public MainMenuUserRunner(Dictionary<int, IUniCommand> commands) => _commands = commands;
    private static readonly Dictionary<int, IUniCommand> _commands = new()
    {
        {1, new FindTracksCommand()} //а как какать? убрать параметры из команд и создать какой-то ридер, через
        //который получаем значение из консоли от пользователя, и юзать его вместо параметров
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