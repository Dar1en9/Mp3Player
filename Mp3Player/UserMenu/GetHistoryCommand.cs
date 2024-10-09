namespace Mp3Player.UserMenu;

public class GetHistoryCommand: AbstractCommand<Task<List<Track>>>
{
    public GetHistoryCommand()
    { 
        Description = "Вывести последние треки из истории поиска";
    }
    public override async Task<List<Track>> Execute()
    {
        return await DataBaseReader.GetProfessorTracks(HistoryManager.GetHistory());
    }
}