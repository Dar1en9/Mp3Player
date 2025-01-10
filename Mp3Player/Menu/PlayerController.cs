using Microsoft.AspNetCore.Mvc;
using Mp3Player.Menu.Commands;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly ICommand<List<Track>, string> _findTracksCommand;
    private readonly IUniCommand<List<Track>> _getAllTracksCommand;
    private readonly IUniCommand<List<Track>> _getHistoryCommand;
    private readonly ICommand<bool, Track> _playCommand;
    private readonly IUniCommand<bool> _pauseCommand;
    private readonly IUniCommand<bool> _resumeCommand;
    private readonly IUniCommand<bool> _stopCommand;
    private readonly ILogger<PlayerController> _logger;

    public PlayerController(
        ICommand<List<Track>, string> findTracksCommand,
        IUniCommand<List<Track>> getAllTracksCommand,
        IUniCommand<List<Track>> getHistoryCommand,
        ICommand<bool, Track> playCommand,
        IUniCommand<bool> pauseCommand,
        IUniCommand<bool> resumeCommand,
        IUniCommand<bool> stopCommand,
        ILogger<PlayerController> logger)
    {
        _findTracksCommand = findTracksCommand;
        _getAllTracksCommand = getAllTracksCommand;
        _getHistoryCommand = getHistoryCommand;
        _playCommand = playCommand;
        _pauseCommand = pauseCommand;
        _resumeCommand = resumeCommand;
        _stopCommand = stopCommand;
        _logger = logger;
    }

    [HttpGet("find")]
    public async Task<IActionResult> FindTracks([FromQuery] string professor)
    {
        _logger.LogDebug("Поиск треков по преподавателю: {Professor}", professor);
        var tracks = await _findTracksCommand.Execute(professor);
        var trackDtos = tracks.Select(track => new TrackDto(track.Id.Id, track.Professor, track.TrackName, track.AudioPath)).ToList();
        return Ok(trackDtos);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllTracks()
    {
        _logger.LogDebug("Получение всех треков");
        var tracks = await _getAllTracksCommand.Execute();
        var trackDtos = tracks.Select(track => new TrackDto(track.Id.Id, track.Professor, track.TrackName, track.AudioPath)).ToList();
        return Ok(trackDtos);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        _logger.LogDebug("Получение истории треков");
        var tracks = await _getHistoryCommand.Execute();
        var trackDtos = tracks.Select(track => new TrackDto(track.Id.Id, track.Professor, track.TrackName, track.AudioPath)).ToList();
        return Ok(trackDtos);
    }

    [HttpPost("play")]
    public async Task<IActionResult> PlayTrack([FromBody] TrackDto trackDto)
    {
        _logger.LogDebug("Воспроизведение трека: {TrackName}", trackDto.TrackName);
        var track = new Track(trackDto.Professor, trackDto.TrackName, new TrackId(trackDto.Id), trackDto.AudioPath);
        await _playCommand.Execute(track);
        return Ok("Трек воспроизводится");
    }

    [HttpPost("pause")]
    public async Task<IActionResult> PauseTrack()
    {
        _logger.LogDebug("Пауза трека");
        await _pauseCommand.Execute();
        return Ok("Трек на паузе");
    }
    
    [HttpPost("resume")]
    public async Task<IActionResult> ResumeTrack()
    {
        _logger.LogDebug("Возобновление трека");
        await _resumeCommand.Execute();
        return Ok("Трек возобновлен");
    }

    [HttpPost("stop")]
    public async Task<IActionResult> StopTrack()
    {
        _logger.LogDebug("Остановка трека");
        await _stopCommand.Execute();
        return Ok("Трек остановлен");
    }
}
