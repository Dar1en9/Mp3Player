using Mp3Player.DataBase;

namespace Mp3Player.UserMenu;

public abstract class AbstractCommand<T>
{
    protected IDataBaseReader DataBaseReader;
    protected IDataBaseWriter DataBaseWriter;
    protected IDataBaseDeleter DataBaseDeleter;
    protected IHistoryManager HistoryManager;
    public string? Description { get; protected init; }
    public abstract T Execute();
}