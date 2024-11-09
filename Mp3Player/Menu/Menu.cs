using Mp3Player.Exceptions;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Buttons;

namespace Mp3Player.Menu;

public class Menu : IMenu
{
    private readonly ICommandReader _commandReader;
    public Dictionary<int, IButton>? Buttons { get; set; }
    private readonly string _label;
    
    public Menu(string label, ICommandReader commandReader)
    {
        _label = label;
        _commandReader = commandReader;
    }
    
    public async Task<IMenu> Run()
    {
        Console.WriteLine("##" + _label);
        var showMenuTask = ShowHelp();
        var buttonClick = ButtonClick(await CommandHandler());
        await Task.WhenAll(showMenuTask, buttonClick);
        return this;
    }
    
    public async Task ButtonClick(IButton button)
    {
        var execute = button.OnClick();
        //залогировать
        await Task.WhenAll(execute); //добавить помимо execute логи
    }

    public async Task<IButton> CommandHandler()
    {
        while (true)
        {
            var key = await _commandReader.GetInput();
            try
            {   
                if (Buttons == null) throw new NullReferenceException();
                if (Buttons.TryGetValue(key, out var button)) return button;
                throw new WrongCommandException();
            }
            catch (WrongCommandException ex)
            {
                //сделать логи
                await Console.Out.WriteLineAsync(ex.Message);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Кажется, в меню нечего делать :(");
                return new Button("", () =>
                {
                    Environment.Exit(0);
                    return Task.CompletedTask;
                });
            }
        }
    }
    public async Task ShowHelp()
    {
        await Console.Out.WriteLineAsync("Введите номер команды:");
        try
        {
            if (Buttons == null) throw new NullReferenceException();
            foreach (var button in Buttons)
                await Console.Out.WriteLineAsync($"{button.Key}: {button.Value.Label}");
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("Кажется, в меню нечего делать :(");
            Environment.Exit(0);
        }
    }
}