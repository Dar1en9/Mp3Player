using Mp3Player.InputReaders;

namespace Mp3Player.TrackHandler;

public class TrackCreator
{
    private readonly IProfessorReader _professorReader;
    private readonly IAudioPathReader _audioPathReader;

    public TrackCreator(IProfessorReader professorReader, IAudioPathReader audioPathReader)
    {
        _professorReader = professorReader;
        _audioPathReader = audioPathReader;
    }

    public async Task<Track> NewTrack()
    {
        var name = await _professorReader.GetInput();
        var audioPath = await _audioPathReader.GetInput();
        return new Track(name, new TrackId(), audioPath);
    }
}