using Mp3Player.Exceptions;
using System.IO;
using System.Text.RegularExpressions;

namespace Mp3Player.InputReaders;

public partial class AudioPathReader : IAudioPathReader
{
    public async Task<string> GetInput()
    {
        while (true)
        {
            await Console.Out.WriteLineAsync("Введите полный путь аудиофайла:");
            var path = await Console.In.ReadLineAsync();
            if (path == null) throw new MissClickException();
            if (MyRegex().IsMatch(path) && File.Exists(path)) return path;
            try
            {
                throw new WrongDirectoryException();
            }
            catch (WrongDirectoryException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }

    [GeneratedRegex(@"^.*\.(mp3|wav|flac|aac)$", RegexOptions.IgnoreCase, "ru-RU")]
    private static partial Regex MyRegex();
}