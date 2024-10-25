namespace Mp3Player.Exceptions;

public class WrongCommandException() : Exception("Неверная команда. Введите 0 для вывода списка доступных команд")
{
    public override string ToString()
    {
        return "Неверная команда";
    }
}