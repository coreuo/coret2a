using Core.Abstract.Attributes;

namespace Packets.Shard.Features;

public interface IFemale
{
    [Flag("Status", 0)]
    bool Female { get; }
}