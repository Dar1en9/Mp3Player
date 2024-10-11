namespace Mp3Player.TrackCreator;

public class TrackId
{
    private Guid Id { get; init; } = Guid.NewGuid();

    public override string ToString() => Id.ToString();
}