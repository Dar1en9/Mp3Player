using Mp3Player.DataBase;
using Mp3Player.Exceptions;
using Mp3Player.TrackHandler;
using TrackRequestChecker = Mp3Player.RequestHelpers.TrackRequestChecker;

namespace Mp3Player.Menu.Commands.AdminCommands;

public class AddTrackCommand: ICommand<bool, TrackCreatorDto>
{
    private readonly IDataBaseWriter _dataBaseWriter;
    private readonly ILogger<AddTrackCommand> _logger;
    public string Description => "Добавить трек";

    public AddTrackCommand(IDataBaseWriter dataBaseWriter, ILogger<AddTrackCommand> logger)
    {
        _dataBaseWriter = dataBaseWriter;
        _logger = logger;
    }
    public async Task<bool> Execute(TrackCreatorDto trackCreatorDto)
    {
        _logger.LogDebug("Выполнение команды: {Description}", Description);
        try
        {
            if (!TrackRequestChecker.CheckProfessor(trackCreatorDto.Professor) ||
                !TrackRequestChecker.CheckTrackName(trackCreatorDto.TrackName) ||
                !TrackRequestChecker.CheckAudioPath(trackCreatorDto.AudioPath))
            {
                _logger.LogDebug("Данные трека не соответствуют формату");
                return false;
            }
            var track = new Track(trackCreatorDto.Professor, trackCreatorDto.TrackName,
                new TrackId(), trackCreatorDto.AudioPath);
            await _dataBaseWriter.WriteTrack(track);
            _logger.LogDebug("Трек успешно добавлен в базу данных");
        }
        catch (MissClickException ex)
        {
            _logger.LogDebug("Ошибка: {Message}", ex.Message);
            return false;
        }

        return true;
    }
}
