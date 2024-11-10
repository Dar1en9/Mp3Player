﻿using System.Reflection;
using Mp3Player.InputReaders;
using Mp3Player.Menu;

namespace Mp3Player;

public class ProgramRunner: IProgramRunner
{
    private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.
        MyMusic), "MP3Player"); //папка MP3Player в папке MyMusic

    public async Task Run()
    {
        if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
        var storagePath = Path.Combine(_path, "Storage");
        if (!Directory.Exists(storagePath)) Directory.CreateDirectory(storagePath);
        var historyPath = Path.Combine(storagePath, "history.txt");
        if (!File.Exists(historyPath)) await File.Create(historyPath).DisposeAsync();
        await new UserMenu(storagePath, historyPath).Run();
    }
}