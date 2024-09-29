namespace Mp3Player.DataBase;

public interface IDataBaseReader
{
    Task<Track> GetTrack(string trackPath);
    Task<List<Track>> GetProfessorTracks(string professorFolder);
    Task<List<Track>> ReadAllTracks();
}