using Mp3Player.Menu.Commands;
using Mp3Player.InputReaders;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.UserMenu;

public class TrackListPage : IMenu
{
    private readonly ICommandReader _commandReader;
    private readonly IMenuNavigator _menuNavigator;
    public List<Track> TrackList { get; set; }
    
    public TrackListPage(ICommandReader commandReader, IMenuNavigator menuNavigator)
    {
        _commandReader = commandReader;
        _menuNavigator = menuNavigator;
    }

    public async Task<IMenu> Run()
    {
        await Console.Out.WriteLineAsync("Список треков по вашему запросу:");
        for (var i = 0; i < TrackList.Count; i++)
        {
            Console.WriteLine($"{i + 1}: {TrackList[i].Professor}"); //
        }
    }

    public Task ExecuteCommand(IUniCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<IUniCommand> CommandHandler()
    {
        throw new NotImplementedException();
    }
}