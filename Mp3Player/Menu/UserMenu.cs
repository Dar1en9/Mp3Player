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

namespace Mp3Player.Menu;

public class UserMenu
{
    private readonly FindTracksCommand _findTrackCommand;
    private readonly GetAllTracksCommand _getAllTracksCommand;
    private readonly GetHistoryCommand _getHistoryCommand;
    private readonly ExitCommand _exitCommand;
    private readonly PlayCommand _playCommand;
    private readonly MenuNavigator _menuNavigator;
    private readonly Menu _trackListPage;
    private readonly Menu _mainMenu;
    private readonly Menu _playerMenu;
    private readonly Player _player;
    private readonly Button _pauseButton;
    private readonly Button _resumeButton;
    private readonly Button _stopButton;

    public UserMenu(string storageDirectory, string historyDirectory)
    {
        var dataBaseReader = new DataBaseReader(storageDirectory);
        var professorReader = new ProfessorReader();
        var commandReader = new CommandReader();
        var historyManager = new HistoryManager(historyDirectory);
        _player = new Player();
        _findTrackCommand = new FindTracksCommand(professorReader, dataBaseReader, historyManager);
        _getAllTracksCommand = new GetAllTracksCommand(dataBaseReader);
        _getHistoryCommand = new GetHistoryCommand(dataBaseReader, historyManager);
        _exitCommand = new ExitCommand();
        _playCommand = new PlayCommand(_player);
        var pauseCommand = new PauseCommand(_player);
        var resumeCommand = new ResumeCommand(_player);
        var stopCommand = new StopCommand(_player);
        _menuNavigator = new MenuNavigator();
        _trackListPage = new Menu("Список треков по вашему запросу", commandReader);
        _mainMenu = new Menu("Главное меню", commandReader);
        _playerMenu = new Menu("Плеер", commandReader);
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
            await _menuNavigator.NavigateTo(_trackListPage);
        });
        Init();
    }
    
    public async Task Run() => await _mainMenu.Run();
    
    private void Init()
    {
        _playCommand.OnPlaybackFinished = OnPlaybackFinished;
        var toMainMenuButton = new Button("В главное меню", async () => 
            await _menuNavigator.NavigateTo(_mainMenu));
        var findTracksButton = new Button(_findTrackCommand.Description,
            async () =>
            {
                try
                {
                    await TracksToButtons(_findTrackCommand, toMainMenuButton);
                }
                catch (MissClickException ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    await _menuNavigator.NavigateTo(_mainMenu);
                }
            });
        var getAllTracksButton = new Button(_getAllTracksCommand.Description, async () => 
            await TracksToButtons(_getAllTracksCommand, toMainMenuButton));
        var getHistoryButton = new Button(_getHistoryCommand.Description, async () =>
            await TracksToButtons(_getHistoryCommand, toMainMenuButton));
        var exitButton = new Button(_exitCommand.Description, async () =>
            await _exitCommand.Execute());
        var mainMenuButtons = new Dictionary<int, IButton>
        {
            {1, findTracksButton},
            {2, getAllTracksButton},
            {3, getHistoryButton},
            {4, exitButton}
        };
        _mainMenu.Buttons = mainMenuButtons;
    }

    private async Task UpdatePlayerButtons()
    {
        _playerMenu.Buttons = new Dictionary<int, IButton>
        {
            {1, _player is { Playing: true, Paused: false } or 
                { Playing: false } ? _pauseButton : _resumeButton},
            {2, _stopButton}
        };
        await _menuNavigator.NavigateTo(_playerMenu);
    }
    
    private async Task OnPlaybackFinished(object? sender, EventArgs e)
    {
        await _playerMenu.Close();
        await _menuNavigator.NavigateTo(_trackListPage, 
            "Воспроизведение завершено. Возвращение к списку треков");
    }

    private async Task TracksToButtons(ICommand<List<Track>, string> command, 
        IButton navigationButton)
    {
        List<Track> tracks;
        try
        {
            tracks = await command.Execute();
        }
        catch (FileNotFoundException)
        {
            //логи сделать
            tracks = [];
        }
        var buttons = tracks.Select(track => 
                new Button($"{track.Professor} — {track.TrackName}", async () => 
                {
                    try
                    {
                        await _playCommand.Execute(track);
                        await UpdatePlayerButtons();
                    }
                    catch (NoDataFoundException ex)
                    {
                        await _menuNavigator.NavigateTo(_trackListPage, ex.Message);
                    }
                }))
            .Select((button, index) => new { button, index })
            .ToDictionary(x => x.index + 1, IButton (x) => x.button);
        buttons.Add(0, navigationButton);
        _trackListPage.Buttons = buttons;
        await _menuNavigator.NavigateTo(_trackListPage);
    }
}