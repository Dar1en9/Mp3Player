using Mp3Player.Exceptions;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Commands;


namespace Mp3Player.Menu.UserMenu;

public class MainMenu : IMenu
{
    private readonly ICommandReader _commandReader;
    private readonly Dictionary<int, IUniCommand> _commands;
    private readonly ShowMenuCommand _showMenu;
    
    public MainMenu(ICommandReader commandReader, Dictionary<int, IUniCommand> commands)
    {
        _commandReader = commandReader;
        _commands = commands;
        _showMenu = new ShowMenuCommand(commands);
    }
    
    public async Task<IMenu> Run()
    {
        Console.WriteLine("##Главное меню");
        var showMenuTask = ExecuteCommand(_showMenu);
        var newCommandTask = ExecuteCommand(await CommandHandler());
        await Task.WhenAll(showMenuTask, newCommandTask);
        return this;
    }
    
    public async Task ExecuteCommand(IUniCommand command)
    {
        var execute = command.Execute();
        //залогировать
        await Task.WhenAll(execute); //добавить помимо execute логи
    }

    public async Task<IUniCommand> CommandHandler()
    {
        while (true)
        {
            var key = await _commandReader.GetInput();
            if (_commands.TryGetValue(key, out var command)) return command;
            try
            {
                throw new WrongCommandException();
            }
            catch (WrongCommandException e)
            {
                //сделать логи
                Console.WriteLine(e.Message);
            }
        }
    }
}