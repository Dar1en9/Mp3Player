using Mp3Player.DataBase;
using Mp3Player.TrackCreator;

namespace Mp3Player.Menu.Commands.UserCommands;

public class FindTracksCommand: ICommand<List<Track>, string>
{
    private readonly string _professor;
    private readonly IDataBaseReader _dataBaseReader;
    public string Description { get; } = "Найти трек по преподавателю";
    
    public FindTracksCommand(string professor, IDataBaseReader dataBaseReader)
    {
        _professor = professor;
        _dataBaseReader = dataBaseReader;
    }
    
    Task IUniCommand<string>.Execute(string? arg)
    {
        return Execute(arg);
    }

    public async Task<List<Track>> Execute(string? arg = default)
    {
        return await _dataBaseReader.GetProfessorTracks(_professor);
    }
}