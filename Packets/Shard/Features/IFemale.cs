using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

public interface IFemale
{
    [Flag("Status", 1)]
    bool Female { get; }
}