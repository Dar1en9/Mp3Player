using Moq;
using Microsoft.Extensions.Logging;
using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.TrackHandler;

namespace UnitTests.Menu.Commands.UserCommands;

public class GetHistoryCommandTest
{
    private readonly Mock<IDataBaseReader> _mockDataBaseReader;
    private readonly Mock<IHistoryManager> _mockHistoryManager;
    private readonly GetHistoryCommand _command;

    public GetHistoryCommandTest()
    {
        _mockDataBaseReader = new Mock<IDataBaseReader>();
        _mockHistoryManager = new Mock<IHistoryManager>();
        Mock<ILogger<GetHistoryCommand>> mockLogger = new();
        _command = new GetHistoryCommand(
            _mockDataBaseReader.Object,
            _mockHistoryManager.Object,
            mockLogger.Object
        );
    }

    [Fact]
    public async Task Execute_ShouldReturnTracks_WhenHistoryIsFound()
    {
        const string history = "Professor A"; 
        var track1 = new Track("Professor A", "Track1", new TrackId(), "AudioPath1");
        var track2 = new Track("Professor A", "Track2", new TrackId(), "AudioPath2");
        var tracks = new List<Track> { track1, track2 };

        _mockHistoryManager.Setup(manager => manager.GetHistory())
            .ReturnsAsync(history);

        _mockDataBaseReader.Setup(reader => reader.GetProfessorTracks(history))
            .ReturnsAsync(tracks);

        var result = await _command.Execute();

        Assert.Equal(tracks, result); 
        _mockHistoryManager.Verify(manager => manager.GetHistory(), Times.Once); 
        _mockDataBaseReader.Verify(reader => reader.GetProfessorTracks(history), Times.Once); 
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenDataBaseReaderThrowsException()
    {
        const string history = "Professor A"; 
        _mockHistoryManager.Setup(manager => manager.GetHistory())
            .ReturnsAsync(history);
        _mockDataBaseReader.Setup(reader => reader.GetProfessorTracks(history))
            .ThrowsAsync(new Exception("Database error"));
        
        var exception = await Assert.ThrowsAsync<Exception>(async () => await _command.Execute());

        Assert.Equal("Database error", exception.Message); 
        _mockHistoryManager.Verify(manager => manager.GetHistory(), Times.Once); 
        _mockDataBaseReader.Verify(reader => reader.GetProfessorTracks(history), Times.Once);
    }
}