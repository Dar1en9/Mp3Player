namespace Mp3Player.TrackHandler;

public class TrackId
{
    private Guid Id { get; init; } = Guid.NewGuid();

    public override string ToString() => Id.ToString();
}