using Microsoft.AspNetCore.Mvc;
using Mp3Player.Menu;
using Mp3Player.Menu.Pages;
using Npgsql;

namespace Mp3Player.Runners;

/*
[ApiController]
[Route("api/run")]
public class ProgramRunnerController : ControllerBase, IProgramRunner
{
    private readonly NpgsqlConnection _connection;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProgramRunnerController> _logger;
    private readonly IMenuNavigator _menuNavigator;
    private readonly HttpClient _httpClient;
    private readonly UserPages _userPages;
    private readonly AdminPages _adminPages;

    public ProgramRunnerController(NpgsqlConnection connection, IConfiguration configuration, ILogger<ProgramRunnerController> logger, IMenuNavigator menuNavigator, HttpClient httpClient, UserPages userPages, AdminPages adminPages)
    {
        _connection = connection;
        _configuration = configuration;
        _logger = logger;
        _menuNavigator = menuNavigator;
        _httpClient = httpClient;
        _userPages = userPages;
        _adminPages = adminPages;
    }

    [HttpPost("run")]
    public async Task<IActionResult> Run()
    {
        _logger.LogDebug("Запуск контроллера ProgramRunner");

        if (_configuration.GetValue<bool>("RunAsAdmin"))
        {
            _logger.LogDebug("Запуск AdminPages через API");
            await _adminPages.Run();
            return Ok("AdminPages запущен");
        }
        _logger.LogDebug("Запуск UserPages через API");
        await _userPages.Run();
        return Ok("UserPages запущен");
    }
}
*/
