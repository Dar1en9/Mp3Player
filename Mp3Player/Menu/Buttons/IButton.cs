using Mp3Player.Menu.Commands;

namespace Mp3Player.Menu.Buttons;

public interface IButton
{
    Task OnClick();
}