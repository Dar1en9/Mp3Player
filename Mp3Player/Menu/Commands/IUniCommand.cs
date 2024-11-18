namespace Mp3Player.Menu.Commands;

public interface IUniCommand
{
    string Description { get; }
    Task Execute();
}