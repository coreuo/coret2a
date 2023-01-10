using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

public interface IPoison
{
    [Flag("Status", 2)]
    bool Poison { get; }
}