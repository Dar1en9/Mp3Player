using Microsoft.Extensions.Logging;
using Mp3Player.DataBase;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Buttons;
using Mp3Player.Menu.Commands;
using Mp3Player.Menu.Commands.AdminCommands;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Pages;

public class AdminPages : IPages
{
    private readonly GetAllTracksCommand _getAllTracksCommand;
    private readonly AddTrackCommand _addTrackCommand;
    private readonly DeleteTrackCommand _deleteTrackCommand;
    private readonly ExitCommand _exitCommand;
    private readonly Menu _mainMenu;
    private readonly ILogger _logger;

    public AdminPages(string storageDirectory, ILogger logger)
    {
        _logger = logger;
        var dataBaseWriter = new DataBaseWriter(storageDirectory, logger);
        var dataBaseReader = new DataBaseReader(storageDirectory, logger);
        var dataBaseDeleter = new DataBaseDeleter(storageDirectory, logger);
        var commandReader = new CommandReader(logger);
        var professorReader = new ProfessorReader(logger);
        var trackNameReader = new TrackNameReader(logger);
        var audioPathReader = new AudioPathReader(logger);
        var idReader = new TrackIdReader(logger);
        var trackCreator = new TrackCreator(professorReader, trackNameReader, audioPathReader, _logger);
        _getAllTracksCommand = new GetAllTracksCommand(dataBaseReader, logger);
        _addTrackCommand = new AddTrackCommand(trackCreator, dataBaseWriter, logger);
        _deleteTrackCommand = new DeleteTrackCommand(idReader, dataBaseDeleter, _logger);
        _exitCommand = new ExitCommand(logger);
        _mainMenu = new Menu("Главное меню", commandReader, logger);
        Init();
    }

    public async Task Run()
    {
        _logger.LogInformation("Запуск главного меню Admin Pages");
        await _mainMenu.Run();
        _logger.LogInformation("Завершение AdminPages");
    }

    public void Init()
    {
        _logger.LogInformation("Инициализация AdminPages");

        var getAllTracksButton = new Button(_getAllTracksCommand.Description, async () =>
        {
            _logger.LogInformation("Выполнение кнопки: {Description}", _getAllTracksCommand.Description);
            var tracks = await _getAllTracksCommand.Execute();
            await Console.Out.WriteLineAsync("Список всех треков:");
            foreach (var track in tracks)
                await Console.Out.WriteLineAsync($"{track.Professor} — {track.TrackName}; ID: {track.Id}");
            await _mainMenu.Run();
        });

        var addTrackButton = new Button(_addTrackCommand.Description, async () =>
        {
            _logger.LogInformation("Выполнение кнопки: {Description}", _addTrackCommand.Description);
            if (await _addTrackCommand.Execute())
                await Console.Out.WriteLineAsync("Трек успешно добавлен");
            await _mainMenu.Run();
        });

        var deleteTrackButton = new Button(_deleteTrackCommand.Description, async () =>
        {
            _logger.LogInformation("Выполнение кнопки: {Description}", _deleteTrackCommand.Description);
            if (await _deleteTrackCommand.Execute())
                await Console.Out.WriteLineAsync("Трек успешно удалён");
            await _mainMenu.Run();
        });

        var exitButton = new Button(_exitCommand.Description, async () =>
        {
            _logger.LogInformation("Выполнение кнопки: {Description}", _exitCommand.Description);
            await _exitCommand.Execute();
        });

        var mainMenuButtons = new Dictionary<int, IButton>
        {
            {1, getAllTracksButton},
            {2, addTrackButton},
            {3, deleteTrackButton},
            {4, exitButton}
        };

        _mainMenu.Buttons = mainMenuButtons;
        _logger.LogInformation("AdminPages инициализирован");
    }
}
