using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.TrackHandler;
using System.Text.Json;
using Dapper;

namespace UnitTests.DataBase;

public class DataBaseWriterTest
{
    private readonly Mock<ILogger<DataBaseWriter>> _mockLogger;
    private readonly Mock<IDataBaseService> _mockDatabaseService;
    public DataBaseWriterTest()
    {
        _mockLogger = new Mock<ILogger<DataBaseWriter>>();
        _mockDatabaseService = new Mock<IDataBaseService>();
    }

    [Fact]
    public async Task WriteTrack_AddsTrackToDatabase()
    {
        var trackId = new TrackId(Guid.NewGuid());
        var track = new Track("TestProff", "trackName", trackId, "pathtoaudio");
        var trackDto = new TrackDto(trackId.Id, track.Professor, track.TrackName, track.AudioPath); 
        _mockDatabaseService.Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<DynamicParameters>())) 
            .ReturnsAsync(1); 
        _mockDatabaseService.Setup(db => db.QueryAsync<TrackDto>(It.IsAny<string>(), It.IsAny<DynamicParameters>())) 
            .ReturnsAsync(new List<TrackDto> { trackDto });

        var writer = new DataBaseWriter(_mockDatabaseService.Object, _mockLogger.Object); 
        await writer.WriteTrack(track);
        
        _mockDatabaseService.Verify(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<DynamicParameters>()), Times.Once);
        var addedTrack = (await _mockDatabaseService.Object
            .QueryAsync<TrackDto>("SELECT * FROM Tracks WHERE Id = @Id", new DynamicParameters(new { Id = trackId.Id })))
            .FirstOrDefault(); 
        Assert.NotNull(addedTrack); 
        Assert.Equal(trackId.Id, addedTrack.Id); 
        Assert.Equal(track.Professor, addedTrack.Professor); 
        Assert.Equal(track.TrackName, addedTrack.TrackName); 
        Assert.Equal(track.AudioPath, addedTrack.AudioPath);
    }
}
