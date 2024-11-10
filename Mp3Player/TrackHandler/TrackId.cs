namespace Mp3Player.TrackHandler;

public class TrackId
{
    private Guid Id { get; } = Guid.NewGuid();

    public override string ToString() => Id.ToString();
}