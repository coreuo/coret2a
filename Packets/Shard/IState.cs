using Core.Abstract.Attributes;
using Packets.Shard.Features;

namespace Packets.Shard;

[Entity("Shard", "State")]
public interface IState<in TData, out TAccount, out TMobile, out TMobileCollection, TMap> : Server.IState<TData>,
    IAccessKey,
    ICharacterSlot,
    ITipRequest,
    IName,
    IPassword,
    ICombat,
    IDirection,
    ITarget,
    IStatus
    where TData : IData
    where TAccount : IAccount<TMobile, TMobileCollection>
    where TMobile : IMobile<TMap>
    where TMobileCollection : ICollection<TMobile>
    where TMap : IMap
{
    TMobile Character { get; }

    TAccount Account { get; }
}