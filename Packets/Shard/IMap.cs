using Core.Abstract.Attributes;
using Packets.Shard.Features;

namespace Packets.Shard;

[Entity("Map")]
public interface IMap :
    IArea
{
}