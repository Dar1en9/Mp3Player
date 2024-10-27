using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.UserCommands;

public interface ITrackListCommand : ICommand<List<Track>, string>;