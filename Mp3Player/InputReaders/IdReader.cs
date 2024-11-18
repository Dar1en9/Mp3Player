using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class IdReader: IReader<string>
{
    public async Task<string> GetInput(CancellationToken cancellationToken = default)
    {
        await Console.Out.WriteLineAsync("Введите id трека для удаления:");
        var id = await Console.In.ReadLineAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(id)) throw new MissClickException(); 
        return id;
    }
}