using Microsoft.Extensions.Logging;
using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.InputReaders;

namespace Mp3Player.Menu.Commands.AdminCommands;

public class DeleteTrackCommand: ICommand<bool, string>
{
    private readonly IDataBaseDeleter _dataBaseDeleter;
    private readonly ILogger<DeleteTrackCommand> _logger;
    public string Description => "Удалить трек";

    public DeleteTrackCommand(IDataBaseDeleter dataBaseDeleter, ILogger<DeleteTrackCommand> logger)
    {
        _dataBaseDeleter = dataBaseDeleter;
        _logger = logger;
    }
    
    public async Task<bool> Execute(string trackId)
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        try
        {
            if (!await _dataBaseDeleter.DeleteTrack(trackId))
            {
                _logger.LogWarning("Трек с идентификатором {TrackId} не найден", trackId);
                throw new NoDataFoundException();
            }
            _logger.LogDebug("Трек с идентификатором {TrackId} успешно удален", trackId);
            return true;
        }
        catch (MissClickException ex)
        {
            _logger.LogDebug("Ошибка: {Message}", ex.Message);
            return false;
        }
        catch (NoDataFoundException ex)
        {
            _logger.LogWarning("Ошибка: {Message}", ex.Message);
            return false;
        }
    }
}