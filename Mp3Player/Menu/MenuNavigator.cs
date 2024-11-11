using Spectre.Console;

namespace Mp3Player.Menu;

public class MenuNavigator : IMenuNavigator
{
    public async Task NavigateTo(IMenu menu) {
        AnsiConsole.Clear();
        await menu.Run();
    }
}