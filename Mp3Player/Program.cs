using Mp3Player.DataBase;
using Mp3Player.History;
using Mp3Player.InputReaders;
using Mp3Player.Menu;
using Mp3Player.Menu.Buttons;
using Mp3Player.Menu.Commands.UserCommands;
using Mp3Player.Menu.Commands;
using Mp3Player.Menu.Commands.PlayerCommands;
using NetCoreAudio;


/*
var commands = [

]
foreach(c in commands) c.historyManager =
*/

//string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), Assembly.GetAssembly(typeof(Program)).GetName().Name);
//var first = new Track("Логинов", new TrackId());
//var sec = new Track("Правдин", 2);
//var third = new Track("Логинов", 3);
//await writer.WriteTrack(first);
//await writer.WriteTrack(sec);
//await writer.WriteTrack(third);
//var getAll = new GetAllTracksCommand();
//Console.WriteLine(await getAll.Execute());
//var list = await reader.ReadAllTracks();
//foreach (var l in list) Console.WriteLine(l);
//var deleter = new DataBaseDeleter(path);
//deleter.DeleteTrack(3);
//var player = new Player();

//paths
string path = @"C:\Users\user\RiderProjects\Mp3Player\Storage";
string historyPath = @"C:\Users\user\RiderProjects\Mp3Player\history.txt";

// workers
var dataBaseWriter = new DataBaseWriter(path);
var dataBaseReader = new DataBaseReader(path);
var dataBaseDeleter = new DataBaseDeleter(path);
var professorReader = new ProfessorReader();
var commandReader = new CommandReader();
var audioPathReader = new AudioPathReader();
var historyManager = new HistoryManager(historyPath);
var player = new Player();

//commands
var findTrackCommand = new FindTracksCommand(professorReader, dataBaseReader, historyManager);
var getAllTracksCommand = new GetAllTracksCommand(dataBaseReader);
var getHistoryCommand = new GetHistoryCommand(dataBaseReader, historyManager);
var exitCommand = new ExitCommand();
var playCommand = new PlayCommand(player);
var pauseCommand = new PauseCommand(player);
var resumeCommand = new ResumeCommand(player);
var stopCommand = new StopCommand(player);


var menuNavigator = new MenuNavigator();
var trackListPage = new Menu("Список треков по вашему запросу", commandReader);
var mainMenu = new Menu("Главное меню", commandReader);
var playerMenu = new Menu("Плеер", commandReader);

async Task OnPlaybackFinished(object? sender, EventArgs e)
{
    await Console.Out.WriteLineAsync("Воспроизведение завершено. Возвращение к списку треков");
    await menuNavigator.NavigateTo(trackListPage);
}
playCommand.OnPlaybackFinished = OnPlaybackFinished;

var toMainMenuButton = new Button("В главное меню", async () => await menuNavigator.NavigateTo(mainMenu));

var findTracksButton = new Button(findTrackCommand.Description, async () =>
{
    var tracks = await findTrackCommand.Execute();
    var buttons = tracks.Select(track => 
            new Button(track.TrackName, async () => //для админа в label пойдет ещё и track.Id.ToString()
            {
                await playCommand.Execute(track); 
                await menuNavigator.NavigateTo(playerMenu); 
            }))
        .Select((button, index) => new { button, index })
        .ToDictionary(x => x.index + 1, IButton (x) => x.button);
    buttons.Add(0, toMainMenuButton);
    trackListPage.Buttons = buttons;
    await menuNavigator.NavigateTo(trackListPage);
});


var getAllTracksButton = new Button(getAllTracksCommand.Description, async () =>
{
    var tracks = await getAllTracksCommand.Execute();
    var buttons = tracks.Select(track => 
            new Button(track.Id.ToString(), async () => 
            {
                await playCommand.Execute(track); 
                await menuNavigator.NavigateTo(playerMenu); 
            }))
        .Select((button, index) => new { button, index })
        .ToDictionary(x => x.index + 1, IButton (x) => x.button);
    buttons.Add(0, toMainMenuButton);
    trackListPage.Buttons = buttons;
    await menuNavigator.NavigateTo(trackListPage);
});
var getHistoryButton = new Button(getHistoryCommand.Description, async () =>
{
    var tracks = await getHistoryCommand.Execute();
    var buttons = tracks.Select(track => 
            new Button(track.Id.ToString(), async () => 
            {
                await playCommand.Execute(track); 
                await menuNavigator.NavigateTo(playerMenu); 
            }))
        .Select((button, index) => new { button, index })
        .ToDictionary(x => x.index + 1, IButton (x) => x.button);
    buttons.Add(0, toMainMenuButton);
    trackListPage.Buttons = buttons;
    await menuNavigator.NavigateTo(trackListPage);
});
var exitButton = new Button(exitCommand.Description, async () =>
{
    await exitCommand.Execute();
});

var mainMenuButtons = new Dictionary<int, IButton>
{
    {1, findTracksButton},
    {2, getAllTracksButton},
    {3, getHistoryButton},
    {4, exitButton}
};
mainMenu.Buttons = mainMenuButtons;


var pauseButton = new Button(pauseCommand.Description, async () =>
{
    await pauseCommand.Execute();
});
var resumeButton = new Button(resumeCommand.Description, async () =>
{
    await resumeCommand.Execute();
});
var stopButton = new Button(stopCommand.Description, async () =>
{
    await stopCommand.Execute();
    await menuNavigator.NavigateTo(trackListPage);
});

var playerButtons = new Dictionary<int, IButton>
{
    {1, pauseButton},
    {2, resumeButton},
    {3, stopButton}
};
playerMenu.Buttons = playerButtons;


    




//await new ShowMenuCommand(dictionary).Execute();

//await writer.WriteTrack(first);
//var tracks = await dataBaseReader.GetProfessorTracks("Логинов");
//foreach (var t in tracks)
//    Console.WriteLine(t);


//var 

