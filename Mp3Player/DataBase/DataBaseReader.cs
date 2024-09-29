using System.Text.Json;

namespace Mp3PlayerApp;

public class DataBaseReader: IDataBaseReader {
    private readonly string _path;

    public DataBaseReader(string path) {
        _path = path;
    }

    public async Task<List<Track>> ReadAllTracks() {
        var professorsFolders = Directory.GetDirectories(_path);
        var tracks = new List<Track>();
        foreach (var professorFolder in professorsFolders)
            tracks.AddRange(await GetProfessorTracks(professorFolder));
        return tracks;
    }

    public async Task<List<Track>> GetProfessorTracks(string professorFolder) {
        var tracks = new List<Track>();
        var tracksPaths = Directory.GetFiles(professorFolder);
        foreach (var trackPath in tracksPaths) 
            tracks.Add(await GetTrack(trackPath));
        return tracks;
    }
    
    public async Task<Track> GetTrack(string trackPath) {
        await using var openStream = File.OpenRead(trackPath);
        return await JsonSerializer.DeserializeAsync<Track>(openStream) ?? throw new InvalidOperationException();
    }
}
