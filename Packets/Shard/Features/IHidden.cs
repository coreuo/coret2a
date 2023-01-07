using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

public interface IHidden
{
    [Flag("Status", 4)]
    bool Hidden { get; }
}