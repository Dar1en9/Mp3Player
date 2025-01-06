using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.TrackHandler;

namespace UnitTests.DataBase;

public class DataBaseReaderTest
{
    private readonly Mock<ILogger<DataBaseReader>> _loggerMock;
    private readonly Mock<IDataBaseService> _mockDatabaseService;
    public DataBaseReaderTest()
    {
        _loggerMock = new Mock<ILogger<DataBaseReader>>();
        _mockDatabaseService = new Mock<IDataBaseService>();    }

    [Fact]
    public async Task ReadAllTracks_ShouldReadTracksFromDatabase()
    {
        var trackId1 = new TrackId(Guid.NewGuid()); 
        var trackId2 = new TrackId(Guid.NewGuid()); 
        var trackDto1 = new TrackDto(trackId1.Id, "TestProff1", "trackName1", "pathtoaudio1"); 
        var trackDto2 = new TrackDto(trackId2.Id, "TestProff2", "trackName2", "pathtoaudio2");
        
        _mockDatabaseService.Setup(db => db.QueryAsync<TrackDto>(It.IsAny<string>(), It.IsAny<DynamicParameters>())) 
            .ReturnsAsync(new List<TrackDto> { trackDto1, trackDto2 });

        var reader = new DataBaseReader(_mockDatabaseService.Object, _loggerMock.Object);
        var tracks = await reader.ReadAllTracks();

        Assert.NotNull(tracks);
        Assert.Equal(2, tracks.Count);
        Assert.Contains(tracks, t => Equals(t.Id, trackId1));
        Assert.Contains(tracks, t => Equals(t.Id, trackId2));
    }

    [Fact]
    public async Task GetProfessorTracks_ShouldReturnTracks_WhenTracksExist()
    {
        var trackId1 = new TrackId(Guid.NewGuid()); 
        var trackDto1 = new TrackDto(trackId1.Id, "TestProff1", "trackName1", "pathtoaudio1"); 
        _mockDatabaseService.Setup(db => db.QueryAsync<TrackDto>(It.IsAny<string>(), It.IsAny<DynamicParameters>())) 
            .ReturnsAsync(new List<TrackDto> { trackDto1 });

        var reader = new DataBaseReader(_mockDatabaseService.Object, _loggerMock.Object);
        var tracks = await reader.GetProfessorTracks("TestProff1");

        Assert.NotNull(tracks);
        Assert.Single(tracks);
        Assert.Equal(trackId1, tracks[0].Id);
    }

    [Fact]
    public async Task GetTrack_ShouldReturnTrack_WhenTrackExists()
    {
        var trackId = new TrackId(Guid.NewGuid()); 
        var trackDto = new TrackDto(trackId.Id, "TestProff", "trackName", "pathtoaudio"); 
        _mockDatabaseService.Setup(db => db.QueryAsync<TrackDto>(It.IsAny<string>(), It.IsAny<DynamicParameters>())) 
            .ReturnsAsync(new List<TrackDto> { trackDto });

        var reader = new DataBaseReader(_mockDatabaseService.Object, _loggerMock.Object);
        var result = await reader.GetTrack(trackId.ToString());
    
        Assert.NotNull(result); 
        Assert.Equal(trackId, result.Id); 
    } 
}

