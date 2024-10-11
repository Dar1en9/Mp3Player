using System.Text.Json;
using Mp3Player.TrackCreator;

namespace Mp3Player.DataBase;

public class DataBaseWriter: IDataBaseWriter {
    private readonly string _path;
    public DataBaseWriter(string path) {
        _path = path;
    }
    public async Task WriteTrack(Track track) {
        var directory = Path.Combine(_path, track.Professor);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        var objectSerialized = JsonSerializer.Serialize(track);
        await File.WriteAllTextAsync(Path.Combine(directory, $"{track.Id}.json"), objectSerialized);
    }
}