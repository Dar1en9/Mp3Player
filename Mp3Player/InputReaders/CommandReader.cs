using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class CommandReader : ICommandReader
{
    public async Task<int> GetInput()
    {
        while (true)
        {
            var command = await Console.In.ReadLineAsync();
            if (int.TryParse(command, out var key) && key >= 0) return key;
            try
            {
                throw new WrongCommandException();
            }
            catch (WrongCommandException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}