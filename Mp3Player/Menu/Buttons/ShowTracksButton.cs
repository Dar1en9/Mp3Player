using Mp3Player.Menu.Commands;
using Mp3Player.Menu.Commands.UserCommands;

namespace Mp3Player.Menu.Buttons;

public class ShowTracksButton : IButton //только наверное лучше на каждый поиск отдельно и потом в mainmenu мы
//делаем не executeCommand() а там OnClick() вызываем
{
    private readonly IMenuNavigator _menuNavigator;
    private readonly ITrackListCommand _command;

    public ShowTracksButton(IMenuNavigator menuNavigator, ITrackListCommand command)
    {
        _menuNavigator = menuNavigator;
        _command = command;
    }
    
    public async Task OnClick()
    {
        _menuNavigator.NavigateToTrackList(await _command.Execute());
    }
}