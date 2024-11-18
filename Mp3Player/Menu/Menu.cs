using System.Reflection.Emit;
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
    private CancellationTokenSource _cancellationTokenSource;
    public Menu(string label, ICommandReader commandReader)
    {
        _label = label;
        _commandReader = commandReader;
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    public async Task<IMenu> Run()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _exitMenu = false;
        while (!_exitMenu)
        {
            await Console.Out.WriteLineAsync("##" + _label);
            await ShowHelp(); 
            //логи показаны кнопки
            Console.WriteLine($"{_label} while");
            var button = await CommandHandler(_cancellationTokenSource.Token);
            Console.WriteLine($"CommandHandler ended in {_label}");
            //логи выбрана кнопка {button.Label}
            if (button != null) await ButtonClick(button);
            else await Console.Out.WriteLineAsync($"button == {button}, _exitMenu is {_exitMenu}");
            //логи Выполнено действие кнопки
        }
        return this;
    }
    
    public async Task ButtonClick(IButton button)
    {
        Console.WriteLine($"ButtonClick on button {button.Label} started in {_label}");
        await button.OnClick();
        //залогировать
        //Task.WhenAll(execute); //добавить помимо execute логи
        Console.WriteLine($"ButtonClick ended in {_label}");
    }

    public async Task<IButton?> CommandHandler(CancellationToken cancellationToken)
    {
        Console.WriteLine($"CommandHandler started in {_label}");
        while (true)
        {
            try
            {
                // if (cancellationToken.IsCancellationRequested)
                // {
                //     await Console.Out.WriteLineAsync($"cancellation requested in {_label}");
                //     return null;
                // }
                var key = await _commandReader.GetInput(cancellationToken, _label);
                Console.WriteLine($"Ввод получен в {_label}");
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
                await Console.Out.WriteLineAsync($"{ex.Message} из menu {_label}, кнопки в котором:");
            }
            catch (OperationCanceledException)
            {
                //логи
                await Console.Out.WriteLineAsync($"OperationCanceledException был словлен в {_label}");
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
        await Task.CompletedTask;
    }
}