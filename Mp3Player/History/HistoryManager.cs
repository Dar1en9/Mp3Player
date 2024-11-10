namespace Mp3Player.History;

public class HistoryManager: IHistoryManager
{
    private readonly string _fullPath;

    public HistoryManager(string path)
    {
        _fullPath = Path.Combine(path, "history.txt");;
    }
    
    public async Task WriteHistory(string keyWord)
    {
        await File.WriteAllTextAsync(_fullPath, keyWord);
    }
    
    public async Task<string> GetHistory() 
    {
        if (!File.Exists(_fullPath)) throw new FileNotFoundException();
        return await File.ReadAllTextAsync(_fullPath);
    }
}