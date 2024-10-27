using Mp3Player.Exceptions;
using Mp3Player.TrackHandler;
using System.Text.RegularExpressions;

namespace Mp3Player.InputReaders;

public partial class ProfessorReader : IReader<string>
{
    private string? Professor { get; set; }
    
    public async Task<string> GetInput()
    {
        while (true)
        {
            await Console.Out.WriteLineAsync("Введите имя преподавателя в формате Фамилия И. О.:");
            var name = await Console.In.ReadLineAsync();
            var regex = MyRegex(); //регулярное выражение на основе паттерна
            if (name != null && regex.IsMatch(name)) return name;
            try
            {
                throw new WrongInputException();
            }
            catch (WrongCommandException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    [GeneratedRegex(@"^[А-ЯЁ][а-яё]+ [А-ЯЁ]\. [А-ЯЁ]\.$")]
    private static partial Regex MyRegex();
}