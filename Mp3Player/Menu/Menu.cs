using Mp3Player.Exceptions;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Buttons;

namespace Mp3Player.Menu;

public class Menu : IMenu
{
    private readonly ICommandReader _commandReader;
    public Dictionary<int, IButton>? Buttons { get; set; }
    private readonly string _label;
    private bool _exitMenu; 
    private readonly CancellationTokenSource _cancellationTokenSource;
    public Menu(string label, ICommandReader commandReader)
    {
        _label = label;
        _commandReader = commandReader;
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    public async Task<IMenu> Run()
    {
        _exitMenu = false;
        await Console.Out.WriteLineAsync("##" + _label);
        await ShowHelp(); 
        //логи показаны кнопки
        while (!_exitMenu)
        {
            var button = await CommandHandler(_cancellationTokenSource.Token);
            //логи выбрана кнопка {button.Label}
            if (button != null) await ButtonClick(button);
            //логи Выполнено действие кнопки
        }

        return this;
    }
    
    public async Task ButtonClick(IButton button)
    {
        var execute = button.OnClick();
        //залогировать
        await Task.WhenAll(execute); //добавить помимо execute логи
    }

    public async Task<IButton?> CommandHandler(CancellationToken cancellationToken)
    {
        while (true)
        {
            try
            {
                var key = await _commandReader.GetInput(cancellationToken);
                //логи что ввод получен
                if (Buttons == null)
                {
                    Console.WriteLine("Кажется, в меню нечего делать :(");
                    //логи
                    return new Button("", () =>
                    {
                        Environment.Exit(0);
                        return Task.CompletedTask;
                    });
                }

                if (Buttons.TryGetValue(key, out var button))
                    //логи
                    return button;
                throw new WrongCommandException();
            }
            catch (WrongCommandException ex)
            {
                //сделать логи
                await Console.Out.WriteLineAsync($"{ex.Message} из menu {_label}");
            }
            catch (OperationCanceledException)
            {
                //логи
                return null;
            }
        }
    }
    public async Task ShowHelp()
    {
        await Console.Out.WriteLineAsync("Введите номер команды:");
        if (Buttons == null)
        {
            Console.WriteLine("Кажется, в меню нечего делать :(");
            Environment.Exit(0);
        }
        foreach (var button in Buttons)
            await Console.Out.WriteLineAsync($"{button.Key}: {button.Value.Label}");
    }
    public async Task Close() 
    { 
        _exitMenu = true; 
        await _cancellationTokenSource.CancelAsync(); 
    }
}