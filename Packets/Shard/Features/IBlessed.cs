using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

public interface IBlessed
{
    [Flag("Status", 3)]
    bool Blessed { get; }
}