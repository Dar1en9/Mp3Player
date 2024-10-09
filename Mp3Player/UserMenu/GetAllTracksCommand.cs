namespace Mp3Player.UserMenu;

public class GetAllTracksCommand: AbstractCommand<Task<bool>>
{
    public GetAllTracksCommand()
    {
        Description = "Вывести все существующие треки";
    }
    public override async Task<bool> Execute()
    {
        await DataBaseReader.ReadAllTracks();
        return false;
    }
}