using Core.Abstract.Attributes;
using Packets.Shard.Features;

namespace Packets.Shard;

[Entity("Mobile")]
public interface IMobile<TMap> : IMobile
    where TMap : IMap
{
    TMap Map { get; }
}

[Entity("Mobile")]
public interface IMobile :
    IPassword,
    ISunlight,
    ILight,
    IWeather,
    IDirection,
    INotoriety,
    IName,
    IStatus,
    IId,
    IBody,
    ILocation,
    IHue,
    IFemale,
    IPoison,
    IBlessed,
    ICombat,
    IHidden
{
}