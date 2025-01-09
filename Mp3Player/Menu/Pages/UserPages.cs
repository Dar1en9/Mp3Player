using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.History;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Buttons;
using Mp3Player.Menu.Commands;
using Mp3Player.Menu.Commands.PlayerCommands;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.TrackHandler;
using NetCoreAudio;
using Npgsql;

namespace Mp3Player.Menu.Pages;

public class UserPages: IPages
{
    private readonly FindTracksCommand _findTrackCommand;
    private readonly GetAllTracksCommand _getAllTracksCommand;
    private readonly GetHistoryCommand _getHistoryCommand;
    private readonly PlayCommand _playCommand;
    private readonly IMenuNavigator _menuNavigator;
    private readonly MenuController _trackListPage;
    private readonly MenuController _mainMenu;
    private readonly MenuController _playerMenu;
    private readonly Player _player;
    private readonly Button _pauseButton;
    private readonly Button _resumeButton;
    private readonly Button _stopButton;
    private readonly ILogger<UserPages> _logger;
    private readonly ILogger<MenuController> _loggerMenu;

    public UserPages( NpgsqlConnection connection, IMenuNavigator menuNavigator, string historyDirectory, ILogger<UserPages> logger, ILogger<MenuController> loggerMenu)
    {
        _logger = logger;
        _loggerMenu = loggerMenu;
        _menuNavigator = menuNavigator;
        var dataBaseReader = new DataBaseReader(new DataBaseService(connection), logger);
        var professorReader = new ProfessorReader(logger);
        var commandReader = new CommandReader(logger);
        if (!Directory.Exists(historyDirectory))
        {
            logger.LogDebug("Создание директории: {Path}", historyDirectory);
            Directory.CreateDirectory(historyDirectory);
        }
        var historyManager = new HistoryManager(historyDirectory, logger);
        _player = new Player();
        _findTrackCommand = new FindTracksCommand(professorReader, dataBaseReader, historyManager, logger);
        _getAllTracksCommand = new GetAllTracksCommand(dataBaseReader, logger);
        _getHistoryCommand = new GetHistoryCommand(dataBaseReader, historyManager, logger);
        _playCommand = new PlayCommand(_player, _logger);
        var pauseCommand = new PauseCommand(_player, _logger);
        var resumeCommand = new ResumeCommand(_player, _logger);
        var stopCommand = new StopCommand(_player, _logger);
        _trackListPage = new MenuController(_loggerMenu);
        _trackListPage.Label = "Список треков по вашему запросу";
        _mainMenu = new MenuController(_loggerMenu);
        _mainMenu.Label = "Главное меню";
        _playerMenu = new MenuController(_loggerMenu);
        _playerMenu.Label = "Плеер";
        _resumeButton = new Button(resumeCommand.Description, async () =>
        {
            await resumeCommand.Execute();
            await UpdatePlayerButtons();
        });
        _pauseButton = new Button(pauseCommand.Description, async () =>
        {
            await pauseCommand.Execute();
            await UpdatePlayerButtons();
        });
        _stopButton = new Button(stopCommand.Description, async () =>
        {
            await stopCommand.Execute();
            await _menuNavigator.NavigateTo(_trackListPage.Label);
        });
        Init();
    }

    public async Task Run()
    {
        _logger.LogDebug("Запуск главного меню User Pages");
        await _mainMenu.Run();
        _logger.LogDebug("Завершение UserPages");
    }

    public void Init()
    {
        _logger.LogDebug("Инициализация UserPages");
        _playCommand.OnPlaybackFinished = OnPlaybackFinished;
        var toMainMenuButton = new Button("В главное меню", async () =>
        {
            _logger.LogDebug("Возвращение в главное меню");
            await _menuNavigator.NavigateTo(_mainMenu.Label);
        });
        var findTracksButton = new Button(_findTrackCommand.Description,
            async () =>
            {
                _logger.LogDebug("Выполнение кнопки: {Description}", _findTrackCommand.Description);
                try
                {
                    await TracksToButtons(_findTrackCommand, toMainMenuButton);
                }
                catch (MissClickException ex)
                {
                    _logger.LogDebug("Ошибка: {Message}", ex.Message);
                    await Console.Out.WriteLineAsync(ex.Message);
                    await _menuNavigator.NavigateTo(_mainMenu.Label);
                }
            });
        var getAllTracksButton = new Button(_getAllTracksCommand.Description, async () =>
        {
            _logger.LogDebug("Выполнение кнопки: {Description}", _getAllTracksCommand.Description);
            await TracksToButtons(_getAllTracksCommand, toMainMenuButton);
        });
        var getHistoryButton = new Button(_getHistoryCommand.Description, async () =>
        {
            _logger.LogDebug("Выполнение кнопки: {Description}", _getHistoryCommand.Description);
            await TracksToButtons(_getHistoryCommand, toMainMenuButton);
        });
        var mainMenuButtons = new Dictionary<int, IButton>
        {
            {1, findTracksButton},
            {2, getAllTracksButton},
            {3, getHistoryButton}
        };
        _mainMenu.Buttons = mainMenuButtons;
        _logger.LogDebug("UserPages инициализирован");
    }

    private async Task UpdatePlayerButtons()
    {
        _logger.LogInformation("Обновление кнопок плеера");
        _playerMenu.Buttons = new Dictionary<int, IButton>
        {
            {1, _player is { Playing: true, Paused: false } or 
                { Playing: false } ? _pauseButton : _resumeButton},
            {2, _stopButton}
        };
        await _menuNavigator.NavigateTo(_playerMenu.Label);
    }
    
    private async Task OnPlaybackFinished(object? sender, EventArgs e)
    {
        _logger.LogInformation("Воспроизведение завершено автоматически");
        await Console.Out.WriteLineAsync("Воспроизведение завершено. Нажмите 'Назад' " +
                                         "для возвращения к списку треков");
    }

    private async Task TracksToButtons(ICommand<List<Track>, string> command, 
        IButton navigationButton)
    {
        List<Track> tracks;
        try
        {
            _logger.LogInformation("Попытка получить треки по кнопке: {Description}", command.Description);
            tracks = await command.Execute();
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning("Ошибка: {Message}", ex.Message);
            tracks = [];
        }
        var buttons = tracks.Select(track => 
                new Button($"{track.Professor} — {track.TrackName}", async () => 
                {
                    try
                    {
                        _logger.LogInformation("Попытка воспроизвести трек: {TrackName}", track.TrackName);
                        await _playCommand.Execute(track);
                        await UpdatePlayerButtons();
                    }
                    catch (NoDataFoundException ex)
                    {
                        _logger.LogWarning("Ошибка: {Message}", ex.Message);
                        await _menuNavigator.NavigateTo(_trackListPage.Label, ex.Message);
                    }
                }))
            .Select((button, index) => new { button, index })
            .ToDictionary(x => x.index + 1, IButton (x) => x.button);
        buttons.Add(0, navigationButton);
        _trackListPage.Buttons = buttons;
        _logger.LogInformation("Обновлены кнопки в списке треков");
        await _menuNavigator.NavigateTo(_trackListPage.Label);
    }
}