using System.Text.Json;
using Microsoft.Extensions.Logging;
using Mp3Player.TrackHandler;

namespace Mp3Player.DataBase;

public class DataBaseWriter: IDataBaseWriter {
    private readonly string _path;
    private readonly ILogger _logger;
    public DataBaseWriter(string path, ILogger logger) {
        _path = path;
        _logger = logger;
    }
    public async Task WriteTrack(Track track) {
        _logger.LogInformation("Запись в базу данных трека с ID: {TrackId}", track.Id);
        var directory = Path.Combine(_path, track.Professor);
        if (!Directory.Exists(directory))
        {
            _logger.LogInformation("Создание директории: {Directory}", directory);
            Directory.CreateDirectory(directory);
        }
        var objectSerialized = JsonSerializer.Serialize(track);
        _logger.LogInformation("Трек сериализован: {SerializedTrack}", objectSerialized);
        var filePath = Path.Combine(directory, $"{track.Id}.json");
        await File.WriteAllTextAsync(filePath, objectSerialized);
        _logger.LogInformation("Трек: {track} Записан в файл: {FilePath}", track, filePath);
    }
}