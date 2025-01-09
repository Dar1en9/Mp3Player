using Microsoft.AspNetCore.Mvc;

namespace Mp3Player.Menu;

public interface IMenuNavigator
{
    Task<IActionResult> NavigateTo(string menuLabel, string? message = default);
}