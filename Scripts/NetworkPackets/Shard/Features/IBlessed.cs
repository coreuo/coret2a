using Core.Abstract.Attributes;

namespace NetworkPackets.Shard.Features;

public interface IBlessed
{
    [Flag("Status", 3)]
    bool Blessed { get; }
}