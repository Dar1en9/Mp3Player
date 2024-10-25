using Mp3Player.Exceptions;

namespace Mp3Player.Menu.Commands;

public class CommandReader
{
    public async Task<int> getCommand()
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