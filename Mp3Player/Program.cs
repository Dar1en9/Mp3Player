﻿using Mp3Player;
using Mp3Player.DataBase;
using Mp3Player.UserMenu;
using NetCoreAudio;

/*
var commands = [

]
foreach(c in commands) c.historyManager =
*/
    
const string path = @"C:\Users\alinf\RiderProjects\Mp3Player\Storage";
//var first = new Track("Логинов", 1);
//var sec = new Track("Правдин", 2);
//var third = new Track("Логинов", 3);
//var writer = new DataBaseWriter(path);
//await writer.WriteTrack(first);
//await writer.WriteTrack(sec);
//await writer.WriteTrack(third);
//var reader = new DataBaseReader(path);
var getAll = new GetAllTracksCommand();
Console.WriteLine(await getAll.Execute());
//var list = await reader.ReadAllTracks();
//foreach (var l in list) Console.WriteLine(l);
//var deleter = new DataBaseDeleter(path);
//deleter.DeleteTrack(3);
//var player = new Player();
