using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.History;

namespace UnitTests.History;

public class HistoryManagerTest
{
    private readonly string _testDirectory;
    private readonly HistoryManager _historyManager;

    public HistoryManagerTest()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "HistoryManagerTest");
        if (!Directory.Exists(_testDirectory)) Directory.CreateDirectory(_testDirectory);
        else { 
            Directory.Delete(_testDirectory, true);
            Directory.CreateDirectory(_testDirectory);
        }
        Mock<ILogger> loggerMock = new();
        _historyManager = new HistoryManager(_testDirectory, loggerMock.Object);
    }

    [Fact]
    public async Task TestWriteHistory()
    {
        var keyWord = "writeHistory";

        await _historyManager.WriteHistory(keyWord);

        var filePath = Path.Combine(_testDirectory, "history.txt");
        var content = await File.ReadAllTextAsync(filePath);
        Assert.Equal(keyWord, content);
    }

    [Fact]
    public async Task TestGetHistory()
    {
        var keyWord = "getHistory";
        await _historyManager.WriteHistory(keyWord);

        var result = await _historyManager.GetHistory();

        Assert.Equal(keyWord, result);
    }
    
}