using Mp3Player.TrackHandler;

namespace Mp3Player.Menu;

public interface IMenuNavigator
{
    void NavigateToTrackList(List<Track> tracks);
    void NavigateToPlayer(Track track);
    void NavigateToMainMenu();
}