using Mp3Player.TrackHandler;

namespace Mp3Player.DataBase;

public interface IDataBaseDeleter
{
    Task<bool> DeleteTrack(string id);
}