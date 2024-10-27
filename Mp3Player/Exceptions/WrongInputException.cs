namespace Mp3Player.Exceptions;

public class WrongInputException() : Exception("Данные введены некорректно. Попробуйте снова")
{
    
    public override string ToString()
    {
        return "Некорректный ввод данных";
    }
}