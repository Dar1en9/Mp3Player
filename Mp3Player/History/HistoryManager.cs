namespace Mp3Player.History;

public class HistoryManager: IHistoryManager
{
    private readonly string _fullPath;

    public HistoryManager(string fullPath)
    {
        _fullPath = fullPath;
    }
    
    public async Task WriteHistory(string keyWord)
    {
        await File.WriteAllTextAsync(_fullPath, keyWord);
    }
    
    public async Task<string> GetHistory()
    {
        return await File.ReadAllTextAsync(_fullPath);
    }
}