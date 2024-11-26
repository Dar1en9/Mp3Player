using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.TrackHandler;

namespace UnitTests.DataBase;

public class DataBaseDeleterTest
{
    private readonly Mock<ILogger> _mockLogger;
    private readonly string _testPath;
    private readonly JsonSerializerOptions _jsonOptions;

    public DataBaseDeleterTest()
    {
        _mockLogger = new Mock<ILogger>();
        _testPath = Path.Combine(Path.GetTempPath(), "TestDatabase");
        Directory.CreateDirectory(_testPath);
        _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new TrackIdJsonConverter(_mockLogger.Object) }
        };
    }

    [Fact]
    public async Task DeleteTrack_RemovesFileAndDirectory()
    {
        var trackId = new TrackId();
        var track = new Track("TestProff", "trackName", trackId, "pathtoaudio");
        var directoryPath = Path.Combine(_testPath, track.Professor);
        var filePath = Path.Combine(directoryPath, $"{track.Id}.json");
        Directory.CreateDirectory(directoryPath);
        await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(track, _jsonOptions));

        var deleter = new DataBaseDeleter(_testPath, _mockLogger.Object);
        
        var result = await deleter.DeleteTrack(track.Id.ToString());

        Assert.True(result, "Track was not deleted successfully.");
        Assert.False(File.Exists(filePath), "File was not deleted.");
        Assert.True(Directory.Exists(directoryPath), "Directory was deleted.");
    }

    [Fact]
    public async Task DeleteTrack_TrackDoesNotExist_LogsWarning()
    {
        var deleter = new DataBaseDeleter(_testPath, _mockLogger.Object);
        const string nonExistentTrackId = "хехе, такого id нет";

        var result = await deleter.DeleteTrack(nonExistentTrackId);

        Assert.False(result, "Expected deletion to fail for non-existent track.");
        
        Directory.Delete(_testPath, true);
    }
}