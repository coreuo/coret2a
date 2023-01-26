using Core.Abstract.Attributes;

namespace Scripts.ShardWeather
{
    [Entity("Mobile")]
    public interface IMobile
    {
        int Id { get; }

        byte Light { get; }

        byte Sunlight { get; }
    }
}
