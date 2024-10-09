namespace Mp3Player;

public class HistoryManager: IHistoryManager
{
    private readonly string _path;

    public HistoryManager(string path)
    {
        _path = path;
    }
    
    public void WriteHistory(string keyWord)
    {
        File.WriteAllText(_path, keyWord);
    }

    public string GetHistory()
    {
        return File.ReadAllText(_path);
    }
}