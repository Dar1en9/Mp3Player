namespace Mp3Player.Menu.Commands;

public interface IUniCommand<in T1>
{
    string? Description { get; }
    Task Execute(T1? arg = default);
}