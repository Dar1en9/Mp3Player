namespace Mp3Player.TrackCreator;

public record TrackId
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
}