using Microsoft.Extensions.Logging;
using Mp3Player.InputReaders;

namespace Mp3Player.TrackHandler;

public class TrackCreator: ITrackCreator
{
    private readonly IProfessorReader _professorReader;
    private readonly IAudioPathReader _audioPathReader;
    private readonly IReader<string> _trackNameReader;
    private readonly ILogger _logger;
    
    public TrackCreator(IProfessorReader professorReader, IReader<string> trackNameReader, 
        IAudioPathReader audioPathReader, ILogger logger)
    {
        _logger = logger;
        _professorReader = professorReader;
        _trackNameReader = trackNameReader;
        _audioPathReader = audioPathReader;
    }

    public async Task<Track> NewTrack()
    {
        _logger.LogInformation("Начало создания нового трека");
        var professor = await _professorReader.GetInput();
        _logger.LogInformation("Получено имя преподавателя: {Professor}", professor);
        var trackName = await _trackNameReader.GetInput();
        _logger.LogInformation("Получено имя трека: {TrackName}", trackName);
        var audioPath = await _audioPathReader.GetInput();
        _logger.LogInformation("Получен путь к аудиофайлу: {AudioPath}", audioPath);
        var track = new Track(professor, trackName, new TrackId(), audioPath);
        _logger.LogInformation("Создан новый трек: {track}", track);
        return track;
    }
}