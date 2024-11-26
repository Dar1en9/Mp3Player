using Microsoft.Extensions.Logging;
using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class TrackNameReader(ILogger logger): IReader<string>
{
    public async Task<string> GetInput(CancellationToken cancellationToken = default)
    {
        logger.LogDebug("Запрос ввода названия трека");
        await Console.Out.WriteLineAsync("Введите название трека:");
        var name = await Console.In.ReadLineAsync(cancellationToken);
        logger.LogDebug("Пользователь ввел название трека: {TrackName}", name);
        if (!string.IsNullOrWhiteSpace(name)) return name;
        logger.LogDebug("Название трека пустое или содержит только пробелы");
        throw new MissClickException();
    }
}