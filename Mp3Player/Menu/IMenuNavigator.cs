using Mp3Player.Menu.UserMenu;

namespace Mp3Player.Menu;

public interface IMenuNavigator
{
    Task NavigateTo(IMenu menu);
}