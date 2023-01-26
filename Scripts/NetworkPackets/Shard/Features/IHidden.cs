using Core.Abstract.Attributes;

namespace NetworkPackets.Shard.Features;

public interface IHidden
{
    [Flag("Status", 7)]
    bool Hidden { get; }
}