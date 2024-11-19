using Microsoft.Extensions.Logging;
using Mp3Player.Exceptions;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Buttons;

namespace Mp3Player.Menu;

public class Menu : IMenu
{
    private readonly ICommandReader _commandReader;
    public Dictionary<int, IButton>? Buttons { get; set; }
    public string Label { get; }
    private bool _exitMenu; 
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ILogger _logger;
    public Menu(string label, ICommandReader commandReader, ILogger logger)
    {
        Label = label;
        _commandReader = commandReader;
        _logger = logger;
        _cancellationTokenSource = new CancellationTokenSource();
    }
    
    public async Task<IMenu> Run()
    {
        _logger.LogInformation("Запуск меню: {Label}", Label);
        _exitMenu = false;
        await Console.Out.WriteLineAsync("##" + Label);
        await ShowHelp(); 
        _logger.LogInformation("Показаны все кнопки меню");
        while (!_exitMenu)
        {
            var button = await CommandHandler(_cancellationTokenSource.Token);
            if (button != null)
            {
                _logger.LogInformation("Выбрана кнопка: {ButtonLabel}", button.Label);
                await ButtonClick(button);
                _logger.LogInformation("Завершено действие кнопки: {ButtonLabel}", button.Label);
            }
        }
        _logger.LogInformation("Меню {Label} завершено", Label);
        return this;
    }
    
    public async Task ButtonClick(IButton button)
    {
        _logger.LogInformation("Обработка нажатия кнопки: {ButtonLabel}", button.Label);
        await button.OnClick();
        _logger.LogInformation("Обработка кнопки {ButtonLabel} выполнена", button.Label);
    }

    public async Task<IButton?> CommandHandler(CancellationToken cancellationToken)
    {
        while (true)
        {
            try
            {
                _logger.LogInformation("Ожидание ввода кнопки в меню: {Label}", Label);
                var key = await _commandReader.GetInput(cancellationToken);
                _logger.LogInformation("Ввод получен: {Key}", key);
                if (Buttons == null)
                {
                    _logger.LogWarning("В меню нет кнопок");
                    Console.WriteLine("Кажется, в меню нечего делать :(");
                    //логи
                    return new Button("", () =>
                    {
                        Environment.Exit(0);
                        return Task.CompletedTask;
                    });
                }

                if (Buttons.TryGetValue(key, out var button))
                {
                    _logger.LogInformation("Кнопка найдена: {ButtonLabel}", button.Label);
                    return button;
                }
                throw new WrongCommandException();
            }
            catch (WrongCommandException ex)
            {
                _logger.LogWarning("Ошибка: {Message}, В меню: {Label}", ex.Message, Label);
                await Console.Out.WriteLineAsync(ex.Message);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Ожидание ввода с консоли отменено");
                return null;
            }
        }
    }
    public async Task ShowHelp()
    {
        _logger.LogInformation("Показ справки для меню: {Label}", Label);
        await Console.Out.WriteLineAsync("Введите номер команды:");
        if (Buttons == null)
        {
            _logger.LogWarning("В меню нет кнопок");
            Console.WriteLine("Кажется, в меню нечего делать :(");
            Environment.Exit(0);
        }

        foreach (var button in Buttons)
        {
            await Console.Out.WriteLineAsync($"{button.Key}: {button.Value.Label}");
            _logger.LogInformation("Показана кнопка: {Key} — {Label}", button.Key, button.Value.Label);
        }
    }
    public async Task Close() 
    { 
        _logger.LogInformation("Закрытие меню: {Label}", Label);
        _exitMenu = true; 
        await _cancellationTokenSource.CancelAsync(); 
        _logger.LogInformation("Меню {Label} закрыто", Label);
    }
}