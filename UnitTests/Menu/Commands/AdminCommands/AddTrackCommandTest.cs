using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.Menu.Commands.AdminCommands;
using Mp3Player.TrackHandler;

namespace UnitTests.Menu.Commands.AdminCommands;
    
public class AddTrackCommandTest
{
    private readonly Mock<ITrackCreator> _trackCreatorMock;
    private readonly Mock<IDataBaseWriter> _dataBaseWriterMock;
    private readonly AddTrackCommand _addTrackCommand;

    public AddTrackCommandTest()
    {
        _trackCreatorMock = new Mock<ITrackCreator>();
        _dataBaseWriterMock = new Mock<IDataBaseWriter>();
        Mock<ILogger> loggerMock = new();
        _addTrackCommand = new AddTrackCommand(_trackCreatorMock.Object, _dataBaseWriterMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task TestAddTrack()
    {
        var track = new Track("TestProfessor", "TestTrackName", new TrackId(), "TestAudioPath");
        _trackCreatorMock.Setup(tc => tc.NewTrack()).ReturnsAsync(track);

        var result = await _addTrackCommand.Execute();
        
        Assert.True(result);
        _dataBaseWriterMock.Verify(db => db.WriteTrack(track), Times.Once);
    }

    [Fact]
    public async Task TestMissClickException()
    {
        _trackCreatorMock.Setup(tc => tc.NewTrack()).ThrowsAsync(new MissClickException());

        var result = await _addTrackCommand.Execute();

        Assert.False(result);
    }
    
}