using Microsoft.Extensions.Logging;
using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.InputReaders;

namespace Mp3Player.Menu.Commands.AdminCommands;

public class DeleteTrackCommand: ICommand<bool, string>
{
    private readonly IReader<string> _idReader;
    private readonly IDataBaseDeleter _dataBaseDeleter;
    private readonly ILogger _logger;
    public string Description => "Удалить трек";

    public DeleteTrackCommand(IReader<string> idReader, IDataBaseDeleter dataBaseDeleter, ILogger logger)
    {
        _idReader = idReader;
        _dataBaseDeleter = dataBaseDeleter;
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
        _logger.LogInformation("Выполнение команды: {Description}", Description);
        try
        {
            var trackId = await _idReader.GetInput();
            _logger.LogInformation("Получен идентификатор трека: {TrackId}", trackId);
            if (!await _dataBaseDeleter.DeleteTrack(trackId))
            {
                _logger.LogWarning("Трек с идентификатором {TrackId} не найден", trackId);
                throw new NoDataFoundException();
            }
            _logger.LogInformation("Трек с идентификатором {TrackId} успешно удален", trackId);
            return true;
        }
        catch (MissClickException ex)
        {
            _logger.LogInformation("Ошибка: {Message}", ex.Message);
            await Console.Out.WriteLineAsync(ex.Message);
            return false;
        }
        catch (NoDataFoundException ex)
        {
            _logger.LogWarning("Ошибка: {Message}", ex.Message);
            await Console.Out.WriteLineAsync(ex.Message);
            return false;
        }
    }
}