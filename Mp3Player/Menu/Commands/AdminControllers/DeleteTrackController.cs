using Microsoft.AspNetCore.Mvc;
using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.InputReaders;

namespace Mp3Player.Menu.Commands.AdminControllers;
/*
[ApiController]
[Route("api/[controller]")]
public class DeleteTrackController : ControllerBase, ICommand<IActionResult, string>
{
    private readonly IReader<string> _idReader;
    private readonly IDataBaseDeleter _dataBaseDeleter;
    private readonly ILogger<DeleteTrackController> _logger;

    public string Description => "Удалить трек";

    public DeleteTrackController(IReader<string> idReader, IDataBaseDeleter dataBaseDeleter, ILogger<DeleteTrackController> logger)
    {
        _idReader = idReader;
        _dataBaseDeleter = dataBaseDeleter;
        _logger = logger;
    }

    Task IUniCommand.Execute()
    {
        _logger.LogWarning("Выполнение команды {Description} было вызвано " +
                           "через универсальный интерфейс IUniCommand", Description);
        return Execute();
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Execute(string? arg = default)
    {
        _logger.LogDebug("Запрос на удаление трека");
        try
        {
            var trackId = await _idReader.GetInput();
            _logger.LogDebug("Получен идентификатор трека: {TrackId}", trackId);
            if (!await _dataBaseDeleter.DeleteTrack(trackId))
            {
                _logger.LogWarning("Трек с идентификатором {TrackId} не найден", trackId);
                throw new NoDataFoundException();
            }
            _logger.LogDebug("Трек с идентификатором {TrackId} успешно удален", trackId);
            return Ok("Трек успешно удален");
        }
        catch (MissClickException ex)
        {
            _logger.LogDebug("Ошибка: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (NoDataFoundException ex)
        {
            _logger.LogWarning("Ошибка: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка при удалении трека: {Message}", ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }
}
*/
