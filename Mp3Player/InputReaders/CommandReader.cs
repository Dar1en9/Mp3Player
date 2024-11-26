using Microsoft.Extensions.Logging;
using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class CommandReader(ILogger logger) : ICommandReader
{
    public async Task<int> GetInput(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            try
            {
                logger.LogDebug("Запрос ввода команды");
                var command = await Console.In.ReadLineAsync(cancellationToken);
                logger.LogDebug("Пользователь ввел команду команду: {Command}", command);
                if (!int.TryParse(command, out var key) || key < 0)
                {
                    logger.LogWarning("Введенная команда: {Command} Не является натуральным числом", command);
                    throw new WrongCommandException();
                }
                logger.LogDebug("Команда успешно преобразована в число: {Key}", key);
                return key;
            }
            catch (WrongCommandException ex)
            {
                logger.LogWarning("Ошибка: {Message}", ex.Message);
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}