namespace Mp3Player.DataBase;

public interface IDataBaseDeleter
{
    Task DeleteTrack(int id);
}