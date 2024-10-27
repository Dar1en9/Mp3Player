using Mp3Player.Exceptions;
using Mp3Player.TrackHandler;
using Mp3Player.InputReaders;
using Mp3Player.Menu.UserMenu;

namespace Mp3Player.Menu;

public class MenuNavigator : IMenuNavigator
{
    private readonly ICommandReader _commandReader;
    private readonly MainMenu _mainMenu;
    private readonly TrackListPage _trackListPage;

    public MenuNavigator(ICommandReader commandReader, MainMenu mainMenu)
    {
        _commandReader = commandReader;
        _mainMenu = mainMenu;
        _trackListPage = new TrackListPage(_commandReader, this);
    }

    public void NavigateToMainMenu()
    {
        _mainMenu.Run().Wait();
    }
    
    public async void NavigateToTrackList(List<Track> tracks)
    {
        _trackListPage.TrackList = tracks;
        trackListPage.ShowAsync(tracks).Wait();
        /*
        await Console.Out.WriteLineAsync("Список треков по вашему запросу:");
        for (var i = 0; i < tracks.Count; i++) Console.WriteLine($"{i + 1}: {tracks[i].Professor}"); 
        //добавить какое-то название сюда чтобы треки были разными

        Console.WriteLine("Введите номер трека для воспроизведения или 0 для возврата в главное меню:");
        while (true)
        {
            try
            {
                var key = await _commandReader.GetInput();
                switch (key)
                {
                    case 0:
                        NavigateToMainMenu();
                        return;
                    case > 0 when key <= tracks.Count:
                        NavigateToPlayer(tracks[key - 1]);
                        return;
                    default:
                        throw new WrongInputException();
                }
            }
            catch (WrongInputException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        */
    }
    
    public void NavigateToPlayer(Track track)
    {
        var playerMenu = new PlayerMenu(_commandReader, track);
        playerMenu.ShowAsync().Wait();
    }
}