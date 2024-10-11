using Mp3Player.TrackCreator;

namespace Mp3Player.DataBase;

public interface IDataBaseWriter
{
    Task WriteTrack(Track track);
}