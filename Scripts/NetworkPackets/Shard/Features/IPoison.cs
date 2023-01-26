using Core.Abstract.Attributes;

namespace NetworkPackets.Shard.Features;

public interface IPoison
{
    [Flag("Status", 2)]
    bool Poison { get; }
}