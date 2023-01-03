using Core.Abstract.Attributes;
using Packets.Shard.Outgoing;
using Packets.Shard.Shared;

namespace Packets.Shard;

[Entity("Mobile")]
public interface IMobile :
    ILoginConfirm,
    IZMove,
    ICredentials,
    ISunlight,
    ILight,
    IEquippedMobile,
    IWeather,
    IDirection,
    INotoriety
{
}