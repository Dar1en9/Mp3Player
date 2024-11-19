using Microsoft.Extensions.Logging;
using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class TrackIdReader(ILogger logger): IReader<string>
{
    public async Task<string> GetInput(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Запрос ввода идентификатора трека");
        await Console.Out.WriteLineAsync("Введите id трека для удаления:");
        var id = await Console.In.ReadLineAsync(cancellationToken);
        logger.LogInformation("Администратор ввел идентификатор трека: {TrackId}", id);
        if (!string.IsNullOrWhiteSpace(id)) return id;
        logger.LogInformation("Идентификатор трека пустой или содержит только пробелы");
        throw new MissClickException();
    }
}