using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class IdReader: IReader<string>
{
    public async Task<string> GetInput()
    {
        await Console.Out.WriteLineAsync("Введите id трека для удаления:");
        var id = await Console.In.ReadLineAsync();
        return id ?? throw new MissClickException();
    }
}