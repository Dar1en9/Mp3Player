using Dapper;
using Microsoft.Extensions.Logging;
using Mp3Player.TrackHandler;
namespace Mp3Player.DataBase;
public class DataBaseReader : IDataBaseReader
{
    private readonly DataBaseService _databaseService;
    private readonly ILogger _logger;

    public DataBaseReader(DataBaseService dbService, ILogger logger)
    {
        _databaseService = dbService;
        _logger = logger;
    }

    public async Task<List<Track>> ReadAllTracks()
    {
        _logger.LogDebug("Чтение всех треков из базы данных");
        var tracks = (await _databaseService.QueryAsync<Track>("SELECT * FROM Tracks")).ToList();
        _logger.LogDebug("Все треки ({amount}) успешно прочитаны", tracks.Count);
        return tracks;
    }

    public async Task<List<Track>> GetProfessorTracks(string professor)
    {
        _logger.LogDebug("Чтение треков для преподавателя: {Professor}", professor);
        var parameters = new DynamicParameters();
        parameters.Add("Professor", professor);
        var tracks = (await _databaseService.QueryAsync<Track>("SELECT * FROM Tracks WHERE Professor = @Professor", parameters)).ToList();
        _logger.LogDebug("Треки для преподавателя {Professor} ({amount}) успешно прочитаны", professor, tracks.Count);
        return tracks;
    }

    public async Task<Track> GetTrack(string id)
    {
        _logger.LogDebug("Чтение трека из базы данных по ID: {TrackId}", id);
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        var track = (await _databaseService.QueryAsync<Track>("SELECT * FROM Tracks WHERE Id = @Id", parameters)).FirstOrDefault();
        if (track == null)
        {
            _logger.LogWarning("Трек с ID {TrackId} не найден", id);
            throw new InvalidOperationException();
        }
        _logger.LogDebug("Трек успешно прочитан: {Track}", track);
        return track;
    }
}