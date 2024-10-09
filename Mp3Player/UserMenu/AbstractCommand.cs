namespace Mp3Player.UserMenu;

public interface AbstractCommand<T, in T1>
{
    public string? Description { get; }
    public abstract Task<T> Execute(T1? arg = default);
    
}