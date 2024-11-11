using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class TrackNameReader: IReader<string>
{
    public async Task<string> GetInput()
    {
        await Console.Out.WriteLineAsync("Введите название трека:");
        var name = await Console.In.ReadLineAsync();
        if (string.IsNullOrWhiteSpace(name)) throw new MissClickException();
        return name;
    }
}