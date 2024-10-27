using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class CommandReader : IReader<int>
{
    public async Task<int> GetInput()
    {
        while (true)
        {
            var command = await Console.In.ReadLineAsync();
            if (int.TryParse(command, out var key)) return key;
            try
            {
                throw new WrongCommandException();
            }
            catch (WrongCommandException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}