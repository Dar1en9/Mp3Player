using Mp3Player.Exceptions;
using System.Text.RegularExpressions;

namespace Mp3Player.InputReaders;

public partial class ProfessorReader : IProfessorReader
{
    public async Task<string> GetInput()
    {
        while (true)
        {
            await Console.Out.WriteLineAsync("Введите имя преподавателя в формате Фамилия И. О.:");
            var name = await Console.In.ReadLineAsync();
            var regex = MyRegex(); //регулярное выражение на основе паттерна
            if (name == null) throw new MissClickException();
            if (regex.IsMatch(name)) return name;
            try
            {
                throw new WrongInputException();
            }
            catch (WrongInputException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }

    [GeneratedRegex(@"^[А-ЯЁ][а-яё]+ [А-ЯЁ]\. [А-ЯЁ]\.$")]
    private static partial Regex MyRegex();
}