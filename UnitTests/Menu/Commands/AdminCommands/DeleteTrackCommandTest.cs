using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Commands.AdminCommands;
using Mp3Player.TrackHandler;

namespace UnitTests.Menu.Commands.AdminCommands;

public class DeleteTrackCommandTest
{
    private readonly Mock<IReader<string>> _mockIdReader;
    private readonly Mock<IDataBaseDeleter> _mockDataBaseDeleter;
    private readonly DeleteTrackCommand _deleteTrackCommand;

    public DeleteTrackCommandTest()
    {
        _mockIdReader = new Mock<IReader<string>>();
        _mockDataBaseDeleter = new Mock<IDataBaseDeleter>();
        Mock<ILogger<DeleteTrackCommand>> mockLogger = new();

        _deleteTrackCommand = new DeleteTrackCommand(_mockIdReader.Object, _mockDataBaseDeleter.Object, mockLogger.Object);
    }

    [Fact]
    public async Task TestDeleteTrackWithExistingId()
    {
        var trackId = new TrackId().ToString();
        _mockIdReader.Setup(reader => reader.GetInput(CancellationToken.None)).ReturnsAsync(trackId);
        _mockDataBaseDeleter.Setup(deleter => deleter.DeleteTrack(trackId)).ReturnsAsync(true);

        var result = await _deleteTrackCommand.Execute();

        Assert.True(result);
    }
    
    [Fact]
    public async Task TestDeleteTrackWithNonExistingId()
    {
        var trackId = new TrackId().ToString();
        _mockIdReader.Setup(reader => reader.GetInput(CancellationToken.None)).ReturnsAsync(trackId);
        _mockDataBaseDeleter.Setup(deleter => deleter.DeleteTrack(trackId)).ReturnsAsync(false);

        var result = await _deleteTrackCommand.Execute();

        Assert.False(result);
    }

    [Fact]
    public async Task TestMissClickException()
    {
        var trackId = new TrackId().ToString();
        _mockIdReader.Setup(reader => reader.GetInput(CancellationToken.None)).ReturnsAsync(trackId);
        _mockDataBaseDeleter.Setup(deleter => deleter.DeleteTrack(trackId)).ThrowsAsync(new MissClickException());

        var result = await _deleteTrackCommand.Execute();

        Assert.False(result);
    }

    [Fact]
    public async Task TestNoDataFoundException()
    {
        var trackId = new TrackId().ToString();
        _mockIdReader.Setup(reader => reader.GetInput(CancellationToken.None)).ReturnsAsync(trackId);
        _mockDataBaseDeleter.Setup(deleter => deleter.DeleteTrack(trackId)).ThrowsAsync(new NoDataFoundException());

        var result = await _deleteTrackCommand.Execute();

        Assert.False(result);
    }
}