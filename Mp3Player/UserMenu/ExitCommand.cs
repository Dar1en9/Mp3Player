namespace Mp3Player.UserMenu;

public class ExitCommand: AbstractCommand<bool>
{
    public ExitCommand()
    {
        Description = "Выйти из приложения";
    }
    public override bool Execute()
    {
        Environment.Exit(0);
        return false;
    }
}