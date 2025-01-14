using Dapper;
using Mp3Player.TrackHandler;

namespace Mp3Player.DataBase;
public class DataBaseWriter : IDataBaseWriter
{
    private readonly IDataBaseService _dbService;
    private readonly ILogger<DataBaseWriter> _logger;

    public DataBaseWriter(IDataBaseService dbService, ILogger<DataBaseWriter> logger)
    {
        _dbService = dbService;
        _logger = logger;
    }

    public async Task WriteTrack(Track track)
    {
        _logger.LogDebug("Запись в базу данных трека с ID: {TrackId}", track.Id);
        const string query = @"
            INSERT INTO Tracks (Id, Professor, TrackName, AudioPath)
            VALUES (@Id, @Professor, @TrackName, @AudioPath)
            ON CONFLICT (Id) DO UPDATE
            SET Professor = EXCLUDED.Professor, TrackName = EXCLUDED.TrackName, AudioPath = EXCLUDED.AudioPath";
        var parameters = new DynamicParameters();
        parameters.Add("Id", track.Id.Id);
        parameters.Add("Professor", track.Professor);
        parameters.Add("TrackName", track.TrackName);
        parameters.Add("AudioPath", track.AudioPath);
        await _dbService.ExecuteAsync(query, parameters);
        _logger.LogDebug("Трек: {track} записан в базу данных", track);
    }
}