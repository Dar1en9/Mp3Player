using System;
using System.IO;
using System.Threading.Tasks;
using Mp3Player.TrackHandler;

namespace Mp3Player.DataBase;

public class DataBaseDeleter: IDataBaseDeleter {
    private readonly string _path;
    
    public DataBaseDeleter(string path) {
        _path = path;
    }

    public async Task<bool> DeleteTrack(string id) {
        var files = Directory.GetFiles(_path, $"{id}.json", SearchOption.AllDirectories);
        if (files.Length == 0) return false; 
        var deleteTasks = files.Select(file => Task.Run(() => File.Delete(file)));
        await Task.WhenAll(deleteTasks);
        return true;
    }
}