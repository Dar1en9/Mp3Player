using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class TrackNameReader: IReader<string>
{
    public async Task<string> GetInput()
    {
        while (true)
        {
            try
            {
                await Console.Out.WriteLineAsync("Введите название трека:");
                var name = await Console.In.ReadLineAsync();
                return name ?? throw new WrongInputException();
            }
            catch (WrongInputException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}