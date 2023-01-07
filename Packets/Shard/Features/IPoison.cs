using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

public interface IPoison
{
    [Flag("Status", 1)]
    bool Poison { get; }
}