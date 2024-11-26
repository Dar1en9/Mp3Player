using Microsoft.Extensions.Logging;
using Moq;
using Mp3Player.Exceptions;
using Mp3Player.InputReaders;

namespace UnitTests.InputReaders;

public class AudioPathReaderTest
{
    private readonly AudioPathReader _audioPathReader;

    public AudioPathReaderTest()
    {
        Mock<ILogger> mockLogger = new();
        _audioPathReader = new AudioPathReader(mockLogger.Object);
    }

    [Fact]
    public async Task GetInput_ValidPath_ReturnsPath()
    {
        var validPath = Path.Combine(Path.GetTempPath(), "audio.mp3");
        var cancellationToken = new CancellationTokenSource().Token;

        var consoleInput = new StringReader(validPath);
        Console.SetIn(consoleInput);
            await File.WriteAllTextAsync(validPath, "Test audio content", cancellationToken);
            var result = await _audioPathReader.GetInput(cancellationToken);

            Assert.Equal(validPath, result);
            if (File.Exists(validPath)) File.Delete(validPath);
    }
}