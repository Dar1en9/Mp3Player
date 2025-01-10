namespace Mp3Player.Menu.Commands;

public interface ICommand<T, in T1>
{
    Task<T> Execute(T1 arg);
}
