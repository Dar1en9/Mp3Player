namespace Mp3Player.Exceptions;

public class WrongCommandException() : Exception("Неверная команда. Попробуйте снова")
{
    public override string ToString()
    {
        return "Неверная команда";
    }
}