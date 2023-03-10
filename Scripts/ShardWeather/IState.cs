using Core.Abstract.Attributes;

namespace Scripts.ShardWeather
{
    [Entity("Shard", "State")]
    public interface IState<TMobile>
        where TMobile : IMobile
    {
        TMobile Character { get; }

        int Target { get; }

        Span<char> Text { get; }

        ushort Length { get; }
    }
}
