using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

public interface IHidden
{
    [Flag("Status", 7)]
    bool Hidden { get; }
}