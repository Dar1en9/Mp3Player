using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.TrackHandler;

namespace UnitTests.DataBase;

public class DataBaseReaderTest
{
    private readonly string _testDirectory;
    private readonly Mock<ILogger> _loggerMock;

    public DataBaseReaderTest()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "DataBaseReaderTest");
        Directory.CreateDirectory(_testDirectory);
        _loggerMock = new Mock<ILogger>();
    }

    [Fact]
    public async Task ReadAllTracks_ShouldReadTracksFromFiles()
    {
        if (Directory.Exists(_testDirectory)) Directory.Delete(_testDirectory, true);
        Directory.CreateDirectory(_testDirectory);
        var trackId1 = new TrackId();
        var trackId2 = new TrackId();
        var track1 = new Track("TestProff1", "trackName1", trackId1, "pathtoaudio1");
        var track2 = new Track("TestProff2", "trackName2", trackId2, "pathtoaudio2");
        var professorFolder1 = Path.Combine(_testDirectory, "Professor1");
        Directory.CreateDirectory(professorFolder1);
        await File.WriteAllTextAsync(Path.Combine(professorFolder1, "track1.json"), JsonSerializer.Serialize(track1));
        await File.WriteAllTextAsync(Path.Combine(professorFolder1, "track2.json"), JsonSerializer.Serialize(track2));
        var reader = new DataBaseReader(_testDirectory, _loggerMock.Object);
        
        var tracks = await reader.ReadAllTracks();

        Assert.NotNull(tracks);
        Assert.Equal(2, tracks.Count);
        Assert.Contains(tracks, t => t.Id.Equals(trackId1));
        Assert.Contains(tracks, t => t.Id.Equals(trackId2));
    }
    
    [Fact]
    public async Task GetProfessorTracks_ShouldReturnTracks_WhenDirectoryExists()
    {
        if (Directory.Exists(_testDirectory)) Directory.Delete(_testDirectory, true);
        Directory.CreateDirectory(_testDirectory);
        var trackId = new TrackId(Guid.NewGuid());
        var track = new Track("TestProff", "trackName", trackId, "pathtoaudio");
    
        var professorFolder = Path.Combine(_testDirectory, "Professor1");
        Directory.CreateDirectory(professorFolder);
        await File.WriteAllTextAsync(Path.Combine(professorFolder, "track.json"), JsonSerializer.Serialize(track));
        var reader = new DataBaseReader(_testDirectory, _loggerMock.Object);
        var tracks = await reader.GetProfessorTracks("Professor1");

        Assert.NotNull(tracks);
        Assert.Single(tracks);
        Assert.Equal(trackId, tracks[0].Id);
    }
    
    [Fact]
    public async Task GetTrack_ShouldReturnTrack_WhenFileExists()
    {
        if (Directory.Exists(_testDirectory)) Directory.Delete(_testDirectory, true);
        Directory.CreateDirectory(_testDirectory);
        var trackId = new TrackId(Guid.NewGuid());
        var track = new Track("TestProff", "trackName", trackId, "pathtoaudio");

        var trackFilePath = Path.Combine(_testDirectory, "track.json");
        await File.WriteAllTextAsync(trackFilePath, JsonSerializer.Serialize(track));
        var reader = new DataBaseReader(_testDirectory, _loggerMock.Object);
        var result = await reader.GetTrack(trackFilePath);

        Assert.NotNull(result);
        Assert.Equal(trackId, result.Id);
    }
}

