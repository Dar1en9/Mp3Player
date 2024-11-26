using Microsoft.Extensions.Logging;
using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.TrackHandler;

namespace Mp3Player.Menu.Commands.AdminCommands;

public class AddTrackCommand: ICommand<bool, string>
{
    private readonly ITrackCreator _trackCreator;
    private readonly IDataBaseWriter _dataBaseWriter;
    private readonly ILogger _logger;
    public string Description => "Добавить трек";

    public AddTrackCommand(ITrackCreator trackCreator, IDataBaseWriter dataBaseWriter, ILogger logger)
    {
        _trackCreator = trackCreator;
        _dataBaseWriter = dataBaseWriter;
        _logger = logger;
    }
    
    Task IUniCommand.Execute()
    {
        _logger.LogWarning("Выполнение команды {Description} было вызвано " +
                           "через универсальный интерфейс IUniCommand", Description);
        return Execute();
    }
    
    public async Task<bool> Execute(string? arg = default)
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        try
        {
            var track = await _trackCreator.NewTrack();
            _logger.LogDebug("Получен новый трек: {Track}", track);
            await _dataBaseWriter.WriteTrack(track);
            _logger.LogDebug("Трек успешно добавлен в базу данных");
        }
        catch (MissClickException ex)
        {
            _logger.LogDebug("Ошибка: {Message}", ex.Message);
            await Console.Out.WriteLineAsync(ex.Message);
            return false;
        }

        return true;
    }
}