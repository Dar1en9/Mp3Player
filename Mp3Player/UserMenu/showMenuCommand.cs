using System.Collections;

namespace Mp3Player.UserMenu;

public class ShowMenuCommand: AbstractCommand<bool>
{
    private readonly Hashtable? _commands;
    public override bool Execute()
    {
        //////выводим доступные команды 
        return true;
    }
}