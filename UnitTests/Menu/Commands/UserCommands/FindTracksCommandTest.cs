using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.TrackHandler;

namespace UnitTests.Menu.Commands.UserCommands;

public class FindTracksCommandTests
{
    private readonly Mock<IProfessorReader> _mockProfessorReader;
    private readonly Mock<IDataBaseReader> _mockDataBaseReader;
    private readonly Mock<IHistoryManager> _mockHistoryManager;
    private readonly FindTracksCommand _command;

    public FindTracksCommandTests()
    {
        _mockProfessorReader = new Mock<IProfessorReader>();
        _mockDataBaseReader = new Mock<IDataBaseReader>();
        _mockHistoryManager = new Mock<IHistoryManager>();
        Mock<ILogger<FindTracksCommand>> mockLogger = new();
        _command = new FindTracksCommand(
            _mockProfessorReader.Object,
            _mockDataBaseReader.Object,
            _mockHistoryManager.Object,
            mockLogger.Object
        );
    }

    [Fact]
    public async Task Execute_ShouldReturnTracks_WhenProfessorIsFound()
    {
        const string professorName = "Professor X";
        var track1 = new Track(professorName, "TestTrack1", new TrackId(), "TestAudioPath");
        var track2 = new Track(professorName, "TestTrack2", new TrackId(), "TestAudioPath");
        var track3 = new Track(professorName, "TestTrack3", new TrackId(), "TestAudioPath");
        var tracks = new List<Track> { track1, track2, track3 };

        _mockProfessorReader.Setup(reader => reader.GetInput(CancellationToken.None))
            .ReturnsAsync(professorName);
        _mockHistoryManager.Setup(manager => manager.WriteHistory(professorName))
            .Returns(Task.CompletedTask);
        _mockDataBaseReader.Setup(reader => reader.GetProfessorTracks(professorName))
            .ReturnsAsync(tracks);

        var result = await _command.Execute();

        Assert.Equal(tracks, result);
        _mockProfessorReader.Verify(reader => reader.GetInput(CancellationToken.None), Times.Once);
        _mockHistoryManager.Verify(manager => manager.WriteHistory(professorName), Times.Once);
        _mockDataBaseReader.Verify(reader => reader.GetProfessorTracks(professorName), Times.Once);
    }

    
    [Fact]
    public async Task TestProfessorInputException()
    {
        _mockProfessorReader.Setup(reader => reader.GetInput(CancellationToken.None))
            .ThrowsAsync(new Exception("Input error"));

        await Assert.ThrowsAsync<Exception>(async () => await _command.Execute());
    }
}