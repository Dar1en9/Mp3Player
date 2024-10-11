using System;
using System.IO;
using System.Threading.Tasks;
namespace Mp3Player.DataBase;

public class DataBaseDeleter {
    private readonly string _path;
    
    public DataBaseDeleter(string path) {
        _path = path;
    }

    public async Task DeleteTrack(int id) {
        var files = Directory.GetFiles(_path, $"{id}.json", SearchOption.AllDirectories);
        var deleteTasks = files.Select(file => Task.Run(() => File.Delete(file)));
        await Task.WhenAll(deleteTasks);
    }
}