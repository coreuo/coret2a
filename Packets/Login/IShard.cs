using Core.Abstract.Attributes;
using Packets.Login.Outgoing;

namespace Packets.Login;

[Entity("Shard", "Server")]
public interface IShard :
    IShardInfo
{
}