namespace Mp3Player.History;

public class HistoryManager: IHistoryManager
{
    private readonly string _path;

    public HistoryManager(string path)
    {
        _path = path;
    }
    
    public async Task WriteHistory(string keyWord)
    {
        await File.WriteAllTextAsync(_path, keyWord);
    }
    
    public async Task<string> GetHistory()
    {
        return await File.ReadAllTextAsync(_path);
    }
}