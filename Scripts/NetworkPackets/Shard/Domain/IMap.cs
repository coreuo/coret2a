using Core.Abstract.Attributes;
using NetworkPackets.Shard.Features;

namespace NetworkPackets.Shard.Domain;

[Entity("Map")]
public interface IMap :
    IArea
{
}