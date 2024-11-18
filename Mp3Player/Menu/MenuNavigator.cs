using Spectre.Console;

namespace Mp3Player.Menu;

public class MenuNavigator : IMenuNavigator
{
    public async Task NavigateTo(IMenu menu, string? message = default)
    {
        //AnsiConsole.Clear();
        if (message is not null) await Console.Out.WriteLineAsync(message);
        await menu.Run();
    }
}