namespace Mp3Player.Exceptions;

public class MissClickException() : Exception("Возврат на прошлую страницу")
{
    public override string ToString()
    {
        return "Ошибочное нажатие";
    }
}