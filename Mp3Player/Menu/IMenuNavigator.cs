namespace Mp3Player.Menu;

public interface IMenuNavigator
{
    Task NavigateTo(IMenu menu, string? message = default);
}