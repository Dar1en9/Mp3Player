﻿using Mp3Player.DataBase;
namespace Mp3Player.UserMenu;

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
    public async Task<List<Track>> Execute(string? arg = default)
    {
        return await _dataBaseReader.GetProfessorTracks(_professor);
    }
}