namespace Mp3Player.Exceptions;

public class WrongDirectoryException() : Exception("Указан неверный путь")
{
    public override string ToString()
    {
        return "Неверный путь";
    }
}