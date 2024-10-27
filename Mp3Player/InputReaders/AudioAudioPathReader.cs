using Mp3Player.Exceptions;
using System.IO;
using System.Text.RegularExpressions;

namespace Mp3Player.InputReaders;

public partial class AudioAudioPathReader : IAudioPathReader
{
    public async Task<string> GetInput()
    {
        while (true)
        {
            await Console.Out.WriteLineAsync("Введите полный путь аудиофайла:");
            var path = await Console.In.ReadLineAsync();
            if (path != null && MyRegex().IsMatch(path) && File.Exists(path)) return path;
            try
            {
                throw new WrongDirectoryException();
            }
            catch (WrongDirectoryException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    [GeneratedRegex(@"^.*\.(mp3|wav|flac|aac)$", RegexOptions.IgnoreCase, "ru-RU")]
    private static partial Regex MyRegex();
}