using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mp3Player.TrackHandler;

namespace Mp3Player.DataBase;

public class DataBaseDeleter: IDataBaseDeleter {
    private readonly string _path;
    private readonly ILogger _logger;
    
    public DataBaseDeleter(string path, ILogger logger) {
        _path = path;
        _logger = logger;
    }

    public async Task<bool> DeleteTrack(string id) {
        _logger.LogInformation("Удаление трека из базы данных по ID: {TrackId}", id);
        var files = Directory.GetFiles(_path, $"{id}.json", SearchOption.AllDirectories);
        if (files.Length == 0)
        {
            _logger.LogWarning("Трека с ID {TrackId} нет в базе данных", id);
            return false;
        } 
        var deleteTasks = files.Select(file =>
        {
            _logger.LogInformation("Удаление файла: {FilePath}", file);
            return Task.Run(() => File.Delete(file));
        });
        await Task.WhenAll(deleteTasks);
        _logger.LogInformation("Трек с ID {TrackId} удален из базы данных", id);
        return true;
    }
}