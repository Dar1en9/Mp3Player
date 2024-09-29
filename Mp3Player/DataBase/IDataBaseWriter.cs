namespace Mp3PlayerApp;

public interface IDataBaseWriter
{
    Task WriteTrack(Track track);
}