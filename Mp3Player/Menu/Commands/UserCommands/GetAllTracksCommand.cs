using Microsoft.Extensions.Logging;
using Mp3Player.DataBase;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.UserCommands;

public class GetAllTracksCommand: IUniCommand<List<Track>>
{
    private readonly IDataBaseReader _dataBaseReader;
    private readonly ILogger<GetAllTracksCommand> _logger;
    public string Description => "Вывести все существующие треки";

    public GetAllTracksCommand(IDataBaseReader dataBaseReader, ILogger<GetAllTracksCommand> logger)
    {
        _dataBaseReader = dataBaseReader;
        _logger = logger;
    }

    public async Task<List<Track>> Execute()
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        var tracks = await _dataBaseReader.ReadAllTracks();
        _logger.LogDebug("Получены все треки ({amount}) из базы данных. Команда {Description} завершила " +
                         "выполнение", tracks.Count, Description);
        return tracks;
    }
}