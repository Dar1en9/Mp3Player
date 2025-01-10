namespace Mp3Player.Menu.Commands;

public interface IUniCommand<T>
{
    string Description { get; }
    Task<T> Execute();
}