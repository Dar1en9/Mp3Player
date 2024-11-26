using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.TrackHandler;

namespace UnitTests.Menu.Commands.UserCommands;

public class GetAllTracksCommandTest
{
    private readonly Mock<IDataBaseReader> _mockDataBaseReader;
    private readonly GetAllTracksCommand _command;

    public GetAllTracksCommandTest()
    {
        _mockDataBaseReader = new Mock<IDataBaseReader>();
        Mock<ILogger<GetAllTracksCommand>> mockLogger = new();
        _command = new GetAllTracksCommand(
            _mockDataBaseReader.Object,
            mockLogger.Object
        );
    }

    [Fact]
    public async Task Execute_ShouldReturnAllTracks_WhenTracksAreFound()
    {
        var track1 = new Track("Professor A", "Track1", new TrackId(), "AudioPath1");
        var track2 = new Track("Professor B", "Track2", new TrackId(), "AudioPath2");
        var track3 = new Track("Professor C", "Track3", new TrackId(), "AudioPath3");
        var tracks = new List<Track> { track1, track2, track3 };

        _mockDataBaseReader.Setup(reader => reader.ReadAllTracks())
            .ReturnsAsync(tracks);

        var result = await _command.Execute();

        Assert.Equal(tracks, result);
        _mockDataBaseReader.Verify(reader => reader.ReadAllTracks(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenDataBaseReaderThrowsException()
    {
        _mockDataBaseReader.Setup(reader => reader.ReadAllTracks())
            .ThrowsAsync(new Exception("Database error"));

        await Assert.ThrowsAsync<Exception>(async () => await _command.Execute());
        _mockDataBaseReader.Verify(reader => reader.ReadAllTracks(), Times.Once);
    }
}