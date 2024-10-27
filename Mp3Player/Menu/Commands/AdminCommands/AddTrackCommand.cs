﻿using Mp3Player.DataBase;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.AdminCommands;

public class AddTrackCommand: ICommand<bool, string>
{
    private readonly Track _track;
    private readonly IDataBaseWriter _dataBaseWriter;
    public string? Description { get; } = "Добавить трек";

    public AddTrackCommand(Track track, IDataBaseWriter dataBaseWriter)
    {
        _track = track;
        _dataBaseWriter = dataBaseWriter;
    }
    
    Task IUniCommand.Execute()
    {
        return Execute();
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        await _dataBaseWriter.WriteTrack(_track);
        return true;
    }
}