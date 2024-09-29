namespace Mp3Player;

public interface IHistoryManager
{
    void WriteHistory(string keyWord);
    string GetHistory();
}
