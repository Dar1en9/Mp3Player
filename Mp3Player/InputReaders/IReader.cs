namespace Mp3Player.InputReaders;

public interface IReader<T>
{
    Task<T> GetInput();
}