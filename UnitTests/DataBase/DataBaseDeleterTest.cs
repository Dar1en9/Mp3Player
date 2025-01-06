using Dapper;
using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.TrackHandler;

namespace UnitTests.DataBase;

public class DataBaseDeleterTest
{
    private readonly Mock<ILogger<DataBaseDeleter>> _mockLogger;
    private readonly Mock<IDataBaseService> _mockDatabaseService;

    public DataBaseDeleterTest()
    {
        _mockLogger = new Mock<ILogger<DataBaseDeleter>>();
        _mockDatabaseService = new Mock<IDataBaseService>();
    }

    [Fact]
    public async Task DeleteTrack_RemovesTrackFromDatabase()
    {
        var trackId = new TrackId(Guid.NewGuid());
        var track = new Track("TestProff", "trackName", trackId, "pathtoaudio");
        _mockDatabaseService.Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<DynamicParameters>())) 
            .ReturnsAsync(1); 
        
        var deleter = new DataBaseDeleter(_mockDatabaseService.Object, _mockLogger.Object); 
        var result = await deleter.DeleteTrack(trackId.ToString());
        
        Assert.True(result, "Track was not deleted successfully."); 
        _mockDatabaseService.Verify(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<DynamicParameters>()), Times.Once); 
    }

    [Fact]
    public async Task DeleteTrack_TrackDoesNotExist_LogsWarning()
    {
        _mockDatabaseService.Setup(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<DynamicParameters>())) 
            .ReturnsAsync(0);
        
        var deleter = new DataBaseDeleter(_mockDatabaseService.Object, _mockLogger.Object); 
        var nonExistentTrackId = Guid.NewGuid().ToString(); 
        var result = await deleter.DeleteTrack(nonExistentTrackId);
        
        Assert.False(result, "Expected deletion to fail for non-existent track."); 
        _mockDatabaseService.Verify(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<DynamicParameters>()), Times.Once);
    }
}
