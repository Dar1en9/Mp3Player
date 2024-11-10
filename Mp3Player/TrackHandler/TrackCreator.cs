using Mp3Player.Exceptions;
using Mp3Player.InputReaders;

namespace Mp3Player.TrackHandler;

public class TrackCreator: ITrackCreator
{
    private readonly IProfessorReader _professorReader;
    private readonly IAudioPathReader _audioPathReader;
    private readonly IReader<string> _trackNameReader;
    
    public TrackCreator(IProfessorReader professorReader, IReader<string> trackNameReader, 
        IAudioPathReader audioPathReader)
    {
        _professorReader = professorReader;
        _trackNameReader = trackNameReader;
        _audioPathReader = audioPathReader;
    }

    public async Task<Track> NewTrack()
    {
        var professor = await _professorReader.GetInput();
        var trackName = await _trackNameReader.GetInput();
        var audioPath = await _audioPathReader.GetInput();
        return new Track(professor, trackName, new TrackId(), audioPath);
    }
}