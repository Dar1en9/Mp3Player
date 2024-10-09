namespace Mp3Player.History;

public interface IHistoryManager
{
    Task WriteHistory(string keyWord);
    Task<string>  GetHistory();
}
