using Mp3Player.Menu.Buttons;

namespace Mp3Player.Menu;

public interface IMenu
{
    public string Label { get; }
    Task<IMenu> Run();
    Task<IButton?> ButtonClick(int key);
    Task<Dictionary<int, string>> ShowHelp();
}