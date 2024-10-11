namespace Mp3Player.TrackCreator;

public record TrackId
{
    public Guid Id { get; init; } = Guid.NewGuid();
}