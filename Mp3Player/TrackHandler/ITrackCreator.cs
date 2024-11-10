namespace Mp3Player.TrackHandler;

public interface ITrackCreator
{
    Task<Track> NewTrack();
}