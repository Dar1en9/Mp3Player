using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class TrackNameReader: IReader<string>
{
    public async Task<string> GetInput(CancellationToken cancellationToken = default)
    {
        await Console.Out.WriteLineAsync("Введите название трека:");
        var name = await Console.In.ReadLineAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(name)) throw new MissClickException();
        return name;
    }
}