using Mp3Player.Exceptions;
using System.IO;

namespace Mp3Player.InputReaders;

public class PathReader : IReader<string>
{
    public async Task<string> GetInput()
    {
        while (true)
        {
            await Console.Out.WriteLineAsync("Введите полный путь файла:");
            var path = await Console.In.ReadLineAsync();
            if (path != null && Path.IsPathFullyQualified(path)) return path;
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
}