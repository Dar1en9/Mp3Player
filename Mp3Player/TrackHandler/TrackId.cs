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
    
    public override bool Equals(object? obj)
    {
        if (obj is TrackId other)
        {
            return Id.Equals(other.Id);
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString() => Id.ToString();
}