using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class CommandReader : ICommandReader
{
    public async Task<int> GetInput(CancellationToken cancellationToken = default)
    {
        while (true)
        {
            try
            {
                var command = await Console.In.ReadLineAsync(cancellationToken);
                if (int.TryParse(command, out var key) && key >= 0) return key;
                throw new WrongCommandException();
            }
            catch (WrongCommandException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}