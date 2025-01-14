using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.TrackHandler;
using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class StreamTrackCommand : ICommand<FileStreamResult, string>
{
    private readonly ILogger<StreamTrackCommand> _logger;
    private readonly IDataBaseReader _databaseReader;
    public string Description => "Воспроизведение";

    public StreamTrackCommand(IDataBaseReader databaseReader, ILogger<StreamTrackCommand> logger)
    {
        _databaseReader = databaseReader;
        _logger = logger;
    }
    
    public async Task<FileStreamResult> Execute(string trackId)
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        Track track;
        try
        {
            track = await _databaseReader.GetTrack(trackId);
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning("Трек с ID {TrackId} не найден в базе данных", trackId);
            throw new KeyNotFoundException("Трек не найден.");
        }
        _logger.LogDebug("Начало воспроизведения трека: {track}", track);
        var contentType = GetMimeType(track.AudioPath);
        var stream = new FileStream(track.AudioPath, FileMode.Open, FileAccess.Read);

        return new FileStreamResult(stream, contentType)
        {
            EnableRangeProcessing = true
        };
    }
    public static string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".ogg" => "audio/ogg",
            ".flac" => "audio/flac",
            ".aac" => "audio/aac",
            ".m4a" => "audio/mp4",
            _ => "application/octet-stream"
        };
    }

}