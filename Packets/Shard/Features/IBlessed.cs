using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

public interface IBlessed
{
    [Flag("Status", 2)]
    bool Blessed { get; }
}