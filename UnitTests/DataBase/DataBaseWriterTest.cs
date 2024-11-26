using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.TrackHandler;
using System.Text.Json;

namespace UnitTests.DataBase;

public class DataBaseWriterTest
{
    private readonly Mock<ILogger> _mockLogger;
    private readonly string _testPath;
    private readonly JsonSerializerOptions _jsonOptions;

    public DataBaseWriterTest()
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
    public async Task WriteTrack_CreatesDirectoryAndFile()
    {
        var writer = new DataBaseWriter(_testPath, _mockLogger.Object);
        var trackId = new TrackId(); 
        var track = new Track("TestProff", "trackName", trackId, "pathtoaudio");

        await writer.WriteTrack(track);

        var directoryPath = Path.Combine(_testPath, track.Professor);
        var filePath = Path.Combine(directoryPath, $"{track.Id}.json");
        
        Assert.True(Directory.Exists(directoryPath), "Directory was not created.");
        Assert.True(File.Exists(filePath), "File was not created.");
        var fileContent = await File.ReadAllTextAsync(filePath);
        var deserializedTrack = JsonSerializer.Deserialize<Track>(fileContent, _jsonOptions);
        Assert.Equal(track, deserializedTrack);

        Directory.Delete(_testPath, true);
    }
}