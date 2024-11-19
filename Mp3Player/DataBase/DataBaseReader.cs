using System.Text.Json;
using Microsoft.Extensions.Logging;
using Mp3Player.TrackHandler;
namespace Mp3Player.DataBase;

public class DataBaseReader: IDataBaseReader {
    private readonly string _path;
    private readonly JsonSerializerOptions _options;
    private readonly ILogger _logger;

    public DataBaseReader(string path, ILogger logger) {
        _path = path;
        _logger = logger;
        _options = new JsonSerializerOptions
        {
            Converters = { new TrackIdJsonConverter(_logger) }
        };
    }

    public async Task<List<Track>> ReadAllTracks() {
        _logger.LogInformation("Чтение всех треков из базы данных");
        var professorsFolders = Directory.GetDirectories(_path);
        var tracks = new List<Track>();
        foreach (var professorFolder in professorsFolders)
        {
            tracks.AddRange(await GetProfessorTracks(professorFolder));
        }
        _logger.LogInformation("Все треки ({amount}) успешно прочитаны", tracks.Count);
        return tracks;
    }

    public async Task<List<Track>> GetProfessorTracks(string professorFolder) {
        _logger.LogInformation("Чтение треков для преподавателя: {ProfessorFolder}", professorFolder);
        var tracks = new List<Track>();
        var directory = Path.Combine(_path, professorFolder);
        if (!Directory.Exists(directory))
        {
            _logger.LogWarning("Директория не найдена: {Directory}", directory);
            return tracks;
        }
        var tracksPaths = Directory.GetFiles(directory);
        foreach (var trackPath in tracksPaths)
            tracks.Add(await GetTrack(trackPath));
        _logger.LogInformation("Треки для преподавателя {ProfessorFolder} ({amount}) успешно прочитаны", 
            professorFolder, tracks.Count);
        return tracks;
    }
    
    public async Task<Track> GetTrack(string trackPath) {
        _logger.LogInformation("Чтение трека из файла: {TrackPath}", trackPath);
        await using var openStream = File.OpenRead(trackPath);
        var track = await JsonSerializer.DeserializeAsync<Track>(openStream, _options) ?? throw new InvalidOperationException();
        _logger.LogInformation("Трек успешно прочитан: {Track}", track);
        return track;
    }
}
