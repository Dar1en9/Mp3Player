namespace Mp3Player.DataBase;

public class DataBaseDeleter {
    private readonly string _path;
    public DataBaseDeleter(string path) {
        _path = path;
    }

    public void DeleteTrack(int id) {
        Directory.GetFiles(_path, $"{id}.json", SearchOption.AllDirectories).ToList().ForEach(File.Delete);
    }
}