using Microsoft.AspNetCore.Mvc;
using Mp3Player.DataBase;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Buttons;
using Mp3Player.Menu.Commands.AdminCommands;
using Mp3Player.Menu.Commands.AdminControllers;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.TrackHandler;
using Npgsql;

namespace Mp3Player.Menu.Pages;

public class AdminPages : IPages
{
    private readonly GetAllTracksCommand _getAllTracksCommand;
    private readonly AddTrackCommand _addTrackCommand;
    private readonly MenuController _mainMenu;
    private readonly ILogger<AdminPages> _logger;
    private readonly HttpClient _httpClient;
    private readonly ILogger<MenuController> _loggerMenu;

    public AdminPages( NpgsqlConnection connection, HttpClient httpClient, ILogger<AdminPages> logger, ILogger<MenuController> loggerMenu)
    {
        _logger = logger;
        _loggerMenu = loggerMenu;
        _httpClient = httpClient;
        var dbService = new DataBaseService(connection);
        var dataBaseWriter = new DataBaseWriter(dbService, logger);
        var dataBaseReader = new DataBaseReader(dbService, logger);
        var dataBaseDeleter = new DataBaseDeleter(dbService, logger);
        var commandReader = new CommandReader(logger);
        var professorReader = new ProfessorReader(logger);
        var trackNameReader = new TrackNameReader(logger);
        var audioPathReader = new AudioPathReader(logger);
        var idReader = new TrackIdReader(logger);
        var trackCreator = new TrackCreator(professorReader, trackNameReader, audioPathReader, _logger);
        _getAllTracksCommand = new GetAllTracksCommand(dataBaseReader, logger);
        _addTrackCommand = new AddTrackCommand(trackCreator, dataBaseWriter, logger);
        _mainMenu = new MenuController(_loggerMenu);
        _mainMenu.Label = "Главное меню";
        Init();
    }

    public async Task Run()
    {
        _logger.LogDebug("Запуск главного меню Admin Pages");
        await _mainMenu.Run();
        _logger.LogDebug("Завершение AdminPages");
    }

    public void Init()
    {
        _logger.LogDebug("Инициализация AdminPages");

        var getAllTracksButton = new Button(_getAllTracksCommand.Description, async () =>
        {
            _logger.LogDebug("Выполнение кнопки: {Description}", _getAllTracksCommand.Description);
            var tracks = await _getAllTracksCommand.Execute();
            await Console.Out.WriteLineAsync("Список всех треков:");
            foreach (var track in tracks)
                await Console.Out.WriteLineAsync($"{track.Professor} — {track.TrackName}; ID: {track.Id}");
            await _mainMenu.Run();
        });

        var addTrackButton = new Button(_addTrackCommand.Description, async () =>
        {
            _logger.LogDebug("Выполнение кнопки: {Description}", _addTrackCommand.Description);
            if (await _addTrackCommand.Execute())
                await Console.Out.WriteLineAsync("Трек успешно добавлен");
            await _mainMenu.Run();
        });

        var deleteTrackButton = new Button("Удалить трек", async () => 
        { 
            _logger.LogDebug("Выполнение кнопки: Удалить трек"); 
            var response = await _httpClient.DeleteAsync("api/deletetrack/delete");
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Трек успешно удален");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync(); 
                _logger.LogError("Ошибка при удалении трека: {Message}", errorMessage);
            } 
            await ShowMainMenu();
        });

        var mainMenuButtons = new Dictionary<int, IButton>
        {
            {1, getAllTracksButton},
            {2, addTrackButton},
            {3, deleteTrackButton}
        };

        _mainMenu.Buttons = mainMenuButtons;
        _logger.LogDebug("AdminPages инициализирован");
    }

    private async Task<IActionResult> ShowMainMenu()
    {
        _logger.LogDebug("Показ главного меню");
        var response = await _httpClient.GetAsync($"api/menu/show?label={_mainMenu.Label}");
        if (response.IsSuccessStatusCode)
        {
            var buttons = await response.Content.ReadFromJsonAsync<Dictionary<int, string>>();
            if (buttons != null) return new JsonResult(new { _mainMenu.Label, buttons });
        }

        _logger.LogError("Ошибка при навигации к меню: Главное меню");
        return new JsonResult(new { message = "Ошибка при навигации к меню." });
    }
}
