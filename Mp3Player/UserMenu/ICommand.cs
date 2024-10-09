namespace Mp3Player.UserMenu;

public interface ICommand<T, in T1>
{
    public string? Description { get; }
    public abstract Task<T> Execute(T1? arg = default);
    
}