namespace Mp3Player.Exceptions;

public class WrongDirectoryException() : Exception("Указан неверный путь к файлу")
{
    public override string ToString()
    {
        return "Неверный путь к файлу";
    }
}