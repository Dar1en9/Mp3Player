using NetCoreAudio;

namespace Mp3Player.Menu.Commands.PlayerCommands;

public class ResumeCommand : ICommand<bool, string>
    {
    private readonly Player _player;
    public string Description => "Возобновить";

    public ResumeCommand(Player player)
    {
        _player = player;
    }
    
    Task IUniCommand.Execute()
    {
        return Execute();
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        try
        {
            await _player.Resume();
            Console.WriteLine("Трек возобновлен");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return true;
    }
}