namespace Mp3Player.Menu.Commands;

public interface ICommand<T, in T1>: IUniCommand
{ 
    new Task<T> Execute(T1? arg = default);
    
}