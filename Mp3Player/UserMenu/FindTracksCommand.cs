namespace Mp3Player.UserMenu;

public class FindTracksCommand: AbstractCommand<Task<List<Track>>>
{
    private readonly string _professor;

    public FindTracksCommand(string professor)
    {
        _professor = professor;
        Description = "Найти трек по фамилии преподавателя";
    }
    public override async Task<List<Track>> Execute()
    {
        return await DataBaseReader.GetProfessorTracks(_professor);
    }
}