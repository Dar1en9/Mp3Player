using Mp3Player.Exceptions;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Mp3Player.InputReaders;

public partial class AudioPathReader(ILogger logger) : IAudioPathReader
{
    public async Task<string> GetInput(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            logger.LogInformation("Запрос ввода полного пути аудиофайла");
            await Console.Out.WriteLineAsync("Введите полный путь аудиофайла:"); 
            var path = await Console.In.ReadLineAsync(cancellationToken);
            logger.LogInformation("Пользователь ввел путь аудиофайла: {AudioPath}", path);
            if (string.IsNullOrWhiteSpace(path))
            {
                logger.LogInformation("Путь аудиофайла пустой или содержит только пробелы");
                throw new MissClickException();
            }

            if (MyRegex().IsMatch(path) && File.Exists(path))
            {
                logger.LogInformation("Путь аудиофайла соответствует формату и файл существует");
                return path;
            }
            try
            {
                throw new WrongDirectoryException();
            }
            catch (WrongDirectoryException ex)
            {
                logger.LogWarning("Ошибка: {Message}", ex.Message);
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }

    [GeneratedRegex(@"^.*\.(mp3|wav|flac|aac)$", RegexOptions.IgnoreCase, "ru-RU")]
    private static partial Regex MyRegex();
}