using Dapper;
using Mp3Player.TrackHandler;
namespace Mp3Player.DataBase;
public class DataBaseReader : IDataBaseReader
{
    private readonly IDataBaseService _databaseService;
    private readonly ILogger<DataBaseReader> _logger;

    public DataBaseReader(IDataBaseService dbService, ILogger<DataBaseReader> logger)
    {
        _databaseService = dbService;
        _logger = logger;
    }

    public async Task<List<Track>> ReadAllTracks()
    {
        _logger.LogDebug("Чтение всех треков из базы данных");
        var dbResult = (await _databaseService.QueryAsync<TrackDto>("SELECT Id, Professor, TrackName, AudioPath FROM Tracks")).ToList();
        var tracks = dbResult.Select(dto => new Track(
            dto.Professor,
            dto.TrackName,
            new TrackId(dto.Id),
            dto.AudioPath
        )).ToList();
        _logger.LogDebug("Все треки ({amount}) успешно прочитаны", tracks.Count);
        return tracks;
    }


    public async Task<List<Track>> GetProfessorTracks(string professor)
    {
        _logger.LogDebug("Чтение треков для преподавателя: {Professor}", professor);
        var parameters = new DynamicParameters();
        parameters.Add("Professor", professor);
        var trackDtos = (await _databaseService.QueryAsync<TrackDto>("SELECT Id, Professor, TrackName, AudioPath FROM Tracks WHERE Professor = @Professor", parameters)).ToList();
        var tracks = trackDtos.Select(dto => new Track(
            dto.Professor,
            dto.TrackName,
            new TrackId(dto.Id),
            dto.AudioPath
        )).ToList();
        _logger.LogDebug("Треки для преподавателя {Professor} ({amount}) успешно прочитаны", professor, tracks.Count);
        return tracks;
    }


    public async Task<Track> GetTrack(string id)
    {
        _logger.LogDebug("Чтение трека из базы данных по ID: {TrackId}", id);
        var parameters = new DynamicParameters();
        parameters.Add("Id", Guid.Parse(id));
        var trackDto = (await _databaseService.QueryAsync<TrackDto>("SELECT Id, Professor, TrackName, AudioPath FROM Tracks WHERE Id = @Id", parameters)).FirstOrDefault();
        if (trackDto == null)
        {
            _logger.LogWarning("Трек с ID {TrackId} не найден", id);
            throw new InvalidOperationException();
        }
        var track = new Track(
            trackDto.Professor,
            trackDto.TrackName,
            new TrackId(trackDto.Id),
            trackDto.AudioPath
        );
        _logger.LogDebug("Трек успешно прочитан: {Track}", track);
        return track;
    }
}