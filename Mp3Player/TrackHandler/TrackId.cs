namespace Mp3Player.TrackHandler;

public class TrackId
{
    public Guid Id { get; }

    public TrackId() // Конструктор по умолчанию для сериализации
    {
        Id = Guid.NewGuid();
    }

    public TrackId(Guid id) // Конструктор для десериализации
    {
        Id = id;
    }

    public override string ToString() => Id.ToString();
}