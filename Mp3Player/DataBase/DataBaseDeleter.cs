namespace Mp3PlayerApp;

public class DataBaseDeleter
{
    private readonly string _path;
    public DataBaseDeleter(string path) {
        _path = path;
    }

    public void DeleteTrack(int id)
    {
        //var trackPath = Path.Combine(_path, "*");
        //Console.WriteLine($"Deleting track: {trackPath}");
        Directory.GetFiles(_path, $"{id}.json", SearchOption.AllDirectories).ToList().ForEach(File.Delete);
    }
}