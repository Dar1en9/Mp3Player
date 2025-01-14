using Microsoft.AspNetCore.Mvc;
using Mp3Player.Menu.Commands;
using Mp3Player.RequestCheckers;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly ICommand<List<Track>, string> _findTracksCommand;
    private readonly IUniCommand<List<Track>> _getAllTracksCommand;
    private readonly IUniCommand<List<Track>> _getHistoryCommand;
    private readonly ICommand<bool, string> _deleteTrackCommand;
    private readonly ICommand<bool, TrackCreatorDto> _addTrackCommand;
    private readonly ICommand<FileStreamResult, string> _playCommand;
    private readonly IUniCommand<bool> _pauseCommand;
    private readonly IUniCommand<bool> _resumeCommand;
    private readonly IUniCommand<bool> _stopCommand;
    private readonly ILogger<PlayerController> _logger;

    public PlayerController(
        [FromKeyedServices("find")] ICommand<List<Track>, string> findTracksCommand,
        [FromKeyedServices("all")] IUniCommand<List<Track>> getAllTracksCommand,
        [FromKeyedServices("history")] IUniCommand<List<Track>> getHistoryCommand,
        [FromKeyedServices("delete")] ICommand<bool, string> deleteTrackCommand,
        [FromKeyedServices("add")] ICommand<bool, TrackCreatorDto> addTrackCommand,
        [FromKeyedServices("play")] ICommand<FileStreamResult, string> playCommand,
        [FromKeyedServices("pause")] IUniCommand<bool> pauseCommand,
        [FromKeyedServices("resume")] IUniCommand<bool> resumeCommand,
        [FromKeyedServices("stop")] IUniCommand<bool> stopCommand,
        ILogger<PlayerController> logger)
    {
        _findTracksCommand = findTracksCommand;
        _getAllTracksCommand = getAllTracksCommand;
        _getHistoryCommand = getHistoryCommand;
        _deleteTrackCommand = deleteTrackCommand;
        _addTrackCommand = addTrackCommand;
        _playCommand = playCommand;
        _pauseCommand = pauseCommand;
        _resumeCommand = resumeCommand;
        _stopCommand = stopCommand;
        _logger = logger;
    }

    [HttpGet("FindTracks")]
    public async Task<IActionResult> FindTracks([FromQuery] string professor)
    {
        _logger.LogDebug("Поиск треков по преподавателю: {Professor}", professor);
        if (!TrackRequestChecker.CheckProfessor(professor))
        {
            _logger.LogDebug("Имя преподавателя не соответствует формату");
            return BadRequest("Имя преподавателя не соответствует формату Фамилия И. О.");
        }
        var tracks = await _findTracksCommand.Execute(professor);
        var tracksDto = tracks.Select(track => new TrackDto(track.Id.Id, track.Professor, track.TrackName, track.AudioPath)).ToList();
        return Ok(tracksDto);
    }

    [HttpGet("GetAllTracks")]
    public async Task<IActionResult> GetAllTracks()
    {
        _logger.LogDebug("Получение всех треков");
        var tracks = await _getAllTracksCommand.Execute();
        var tracksDto = tracks.Select(track => new TrackDto(track.Id.Id, track.Professor, track.TrackName, track.AudioPath)).ToList();
        return Ok(tracksDto);
    }

    [HttpGet("GetHistory")]
    public async Task<IActionResult> GetHistory()
    {
        _logger.LogDebug("Получение истории треков");
        var tracks = await _getHistoryCommand.Execute();
        var tracksDto = tracks.Select(track => new TrackDto(track.Id.Id, track.Professor, track.TrackName, track.AudioPath)).ToList();
        return Ok(tracksDto);
    }
    
    [HttpPost("DeleteTrack")]
    public async Task<IActionResult> DeleteTrack([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            _logger.LogDebug("Пустой id");
            return BadRequest("Введен пустой id");
        }
        _logger.LogDebug("Удаление трека");
        if (!Guid.TryParse(id, out _))
        {
            _logger.LogDebug("Введен некорректный формат id");
            return BadRequest("Некорректный формат id");
        }

        var result = await _deleteTrackCommand.Execute(id);
        if (result) return Ok("Трек удален");
        return BadRequest("Ошибка при удалении трека");
    }
    
    [HttpPost("AddTrack")]
    public async Task<IActionResult> AddTrack([FromBody] TrackCreatorDto trackCreatorDto)
    {
        _logger.LogDebug("Добавление трека");
        var result = await _addTrackCommand.Execute(trackCreatorDto);
        if (result) return Ok("Трек добавлен");
        return BadRequest("Данные трека не соответствуют формату");
    }

    [HttpGet("StreamTrack")]
    public async Task<IActionResult> StreamTrack([FromQuery] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            _logger.LogDebug("Пустой id");
            return BadRequest("Введен пустой id");
        }
        if (!Guid.TryParse(id, out _))
        {
            _logger.LogDebug("Введен некорректный формат id");
            return BadRequest("Некорректный формат id");
        }
        try
        {
            var fileStreamResult = await _playCommand.Execute(id);
            return fileStreamResult;
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Трек с таким ID не найден.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при воспроизведении трека");
            return StatusCode(500, "Ошибка на сервере.");
        }
    }
}
