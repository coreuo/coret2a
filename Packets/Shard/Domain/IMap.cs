using Core.Abstract.Attributes;
using Packets.Shard.Features;

namespace Packets.Shard.Domain;

[Entity("Map")]
public interface IMap :
    IArea
{
}