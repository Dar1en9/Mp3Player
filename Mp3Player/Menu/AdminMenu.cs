﻿using Mp3Player.DataBase;
using Mp3Player.InputReaders;
using Mp3Player.Menu.Buttons;
using Mp3Player.Menu.Commands;
using Mp3Player.Menu.Commands.AdminCommands;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu;

public class AdminMenu
{
    private readonly GetAllTracksCommand _getAllTracksCommand;
    private readonly AddTrackCommand _addTrackCommand;
    private readonly DeleteTrackCommand _deleteTrackCommand;
    private readonly ExitCommand _exitCommand;
    private readonly Menu _mainMenu;

    public AdminMenu(string storageDirectory)
    {
        var dataBaseWriter = new DataBaseWriter(storageDirectory);
        var dataBaseReader = new DataBaseReader(storageDirectory);
        var dataBaseDeleter = new DataBaseDeleter(storageDirectory);
        var commandReader = new CommandReader();
        var professorReader = new ProfessorReader();
        var trackNameReader = new TrackNameReader();
        var audioPathReader = new AudioPathReader();
        var idReader = new IdReader();
        var trackCreator = new TrackCreator(professorReader, trackNameReader, audioPathReader);
        _getAllTracksCommand = new GetAllTracksCommand(dataBaseReader);
        _addTrackCommand = new AddTrackCommand(trackCreator, dataBaseWriter);
        _deleteTrackCommand = new DeleteTrackCommand(idReader, dataBaseDeleter);
        _exitCommand = new ExitCommand();
        _mainMenu = new Menu("Главное меню", commandReader);
        Init();
    }
    
    public async Task Run() => await _mainMenu.Run();

    private void Init()
    {
        var getAllTracksButton = new Button(_getAllTracksCommand.Description, async () =>
        {
            await _getAllTracksCommand.Execute();
            await _mainMenu.Run();
        });
        var addTrackButton = new Button("Добавить трек",async () =>
        {
            await _addTrackCommand.Execute();
            await _mainMenu.Run();
        });
        var deleteTrackButton = new Button("Удалить трек",async () =>
            {
            await _deleteTrackCommand.Execute();
            await _mainMenu.Run();
            });
        var exitButton = new Button(_exitCommand.Description, async () =>
            await _exitCommand.Execute());
        var mainMenuButtons = new Dictionary<int, IButton>
        {
            {1, getAllTracksButton},
            {2, addTrackButton},
            {3, deleteTrackButton},
            {4, exitButton}
        };
        _mainMenu.Buttons = mainMenuButtons;
    }
}