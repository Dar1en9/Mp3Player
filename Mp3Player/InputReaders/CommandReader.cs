using Mp3Player.Exceptions;

namespace Mp3Player.InputReaders;

public class CommandReader : ICommandReader
{
    public async Task<int> GetInput(CancellationToken cancellationToken = default, string? from = default) 
    {
        Console.WriteLine($"GetInput started in {from}");
        while (true)
        {
            var readLineTask = Task.Run(async () => await Console.In.ReadLineAsync(cancellationToken), cancellationToken);
            var completedTask = await Task.WhenAny(readLineTask, Task.Delay(Timeout.Infinite, cancellationToken));
            try
            {
                // var thread = new Thread(async () =>
                // {
                //     await Console.In.ReadLineAsync(cancellationToken);
                // });
                
                /*
                var command = Console.In.ReadLineAsync(cancellationToken).GetAwaiter().GetResult();
                if (int.TryParse(command, out var key) && key >= 0) return key;
                throw new WrongCommandException();*/
                if (completedTask == readLineTask)
                {
                    var command = await readLineTask; 
                    if (int.TryParse(command, out var key) && key >= 0) return key; 
                    throw new WrongCommandException();
                } else { throw new OperationCanceledException(cancellationToken); }
            }
            catch (WrongCommandException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("Было выброшено OperationCanceledException и проброшено вверх");
                throw;
            }
            finally
            {
                Console.WriteLine($"GetInput ended in {from}");
            }
        }
    }
}