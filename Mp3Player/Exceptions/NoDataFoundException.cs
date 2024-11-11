namespace Mp3Player.Exceptions;

public class NoDataFoundException() : Exception("Данные не найдены")
{
    public override string ToString()
    {
        return "Нет данных";
    }
}