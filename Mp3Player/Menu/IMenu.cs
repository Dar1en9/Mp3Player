using Microsoft.AspNetCore.Mvc;
using Mp3Player.Menu.Buttons;

namespace Mp3Player.Menu;

public interface IMenu
{
    public string Label { get; }
    Task<IActionResult> Run();
    Task<IButton?> ButtonClick(int key);
    Task<Dictionary<int, string>> ShowHelp();
}