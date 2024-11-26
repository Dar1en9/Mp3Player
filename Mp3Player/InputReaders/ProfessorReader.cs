using Mp3Player.Exceptions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Mp3Player.InputReaders;

public partial class ProfessorReader(ILogger logger) : IProfessorReader
{
    public async Task<string> GetInput(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            logger.LogDebug("Запрос ввода имени преподавателя");
            await Console.Out.WriteLineAsync("Введите имя преподавателя в формате Фамилия И. О.:");
            var name = await Console.In.ReadLineAsync(cancellationToken);
            logger.LogDebug("Пользователь ввел имя преподавателя: {ProfessorName}", name);
            var regex = MyRegex(); //регулярное выражение на основе паттерна
            if (string.IsNullOrWhiteSpace(name))
            {
                logger.LogDebug("Имя преподавателя пустое или содержит только пробелы");
                throw new MissClickException();
            }

            if (regex.IsMatch(name))
            {
                logger.LogDebug("Имя преподавателя соответствует формату");
                return name;
            }
            try
            {
                throw new WrongInputException();
            }
            catch (WrongInputException ex)
            {
                logger.LogWarning("Ошибка: {Message}", ex.Message);
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }

    [GeneratedRegex(@"^[А-ЯЁ][а-яё]+ [А-ЯЁ]\. [А-ЯЁ]\.$")]
    private static partial Regex MyRegex();
}