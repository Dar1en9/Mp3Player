using System.Data;
using Dapper;

namespace Mp3Player.TrackHandler;

public class TrackIdTypeHandler : SqlMapper.TypeHandler<TrackId>
{
    public override void SetValue(IDbDataParameter parameter, TrackId? value)
    {
        parameter.Value = value?.Id;
    }

    public override TrackId Parse(object value)
    {
        return new TrackId((Guid)value);
    }
}
