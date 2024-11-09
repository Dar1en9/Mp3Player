using Mp3Player.DataBase;
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
    private readonly string _path; //= @"C:\Users\user\RiderProjects\Mp3Player\Storage";
    private readonly string _historyPath; //= @"C:\Users\user\RiderProjects\Mp3Player\history.txt";
    private readonly FindTracksCommand _findTrackCommand;
    private readonly GetAllTracksCommand _getAllTracksCommand;
    private readonly GetHistoryCommand _getHistoryCommand;
    private readonly ExitCommand _exitCommand;
    private readonly PlayCommand _playCommand;
    private readonly PauseCommand _pauseCommand;
    private readonly ResumeCommand _resumeCommand;
    private readonly StopCommand _stopCommand;
    private readonly MenuNavigator _menuNavigator;
    private readonly Menu _trackListPage;
    private readonly Menu _mainMenu;
    private readonly Menu _playerMenu;

    public UserMenu(string storagePath, string historyPath)
    {
        _path = storagePath;
        _historyPath = historyPath;
        var dataBaseReader = new DataBaseReader(_path);
        var professorReader = new ProfessorReader();
        var commandReader = new CommandReader();
        var historyManager = new HistoryManager(_historyPath);
        var player = new Player();
        _findTrackCommand = new FindTracksCommand(professorReader, dataBaseReader, historyManager);
        _getAllTracksCommand = new GetAllTracksCommand(dataBaseReader);
        _getHistoryCommand = new GetHistoryCommand(dataBaseReader, historyManager);
        _exitCommand = new ExitCommand();
        _playCommand = new PlayCommand(player);
        _pauseCommand = new PauseCommand(player);
        _resumeCommand = new ResumeCommand(player);
        _stopCommand = new StopCommand(player);
        _menuNavigator = new MenuNavigator();
        _trackListPage = new Menu("Список треков по вашему запросу", commandReader);
        _mainMenu = new Menu("Главное меню", commandReader);
        _playerMenu = new Menu("Плеер", commandReader);
        Init();
    }
    
    public async Task Run() => await _mainMenu.Run();
    
    private void Init()
    {
        _playCommand.OnPlaybackFinished = OnPlaybackFinished;
        var toMainMenuButton = new Button("В главное меню", async () => await _menuNavigator.NavigateTo(_mainMenu));
        var findTracksButton = new Button(_findTrackCommand.Description, async () =>
            await TracksToButtons(_findTrackCommand, toMainMenuButton));
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
    
        var pauseButton = new Button(_pauseCommand.Description, async () =>
            await _pauseCommand.Execute());
        var resumeButton = new Button(_resumeCommand.Description, async () =>
            await _resumeCommand.Execute());
        var stopButton = new Button(_stopCommand.Description, async () =>
        {
            await _stopCommand.Execute();
            await _menuNavigator.NavigateTo(_trackListPage);
        });
        var playerButtons = new Dictionary<int, IButton>
        {
            {1, pauseButton},
            {2, resumeButton},
            {3, stopButton}
        };
        _playerMenu.Buttons = playerButtons; 
    }
    
    private async Task OnPlaybackFinished(object? sender, EventArgs e)
    {
        await Console.Out.WriteLineAsync("Воспроизведение завершено. Возвращение к списку треков");
        await _menuNavigator.NavigateTo(_trackListPage);
    }

    private async Task TracksToButtons(ICommand<List<Track>, string> command, 
        IButton navigationButton)
    {
        var tracks = await command.Execute();
        var buttons = tracks.Select(track => 
                new Button(track.TrackName, async () => //для админа в label пойдет ещё и track.Id.ToString()
                {
                    await _playCommand.Execute(track); 
                    await _menuNavigator.NavigateTo(_playerMenu); 
                }))
            .Select((button, index) => new { button, index })
            .ToDictionary(x => x.index + 1, IButton (x) => x.button);
        buttons.Add(0, navigationButton);
        _trackListPage.Buttons = buttons;
        await _menuNavigator.NavigateTo(_trackListPage);
    }
}