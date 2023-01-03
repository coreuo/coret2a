using Core.Abstract.Attributes;
using Packets.Shard.Incoming;
using Packets.Shard.Outgoing;
using Packets.Shard.Shared;

namespace Packets.Shard;

[Entity("Shard", "State")]
public interface IState<in TData, out TAccount, out TMobile, out TMobileCollection> : Server.IState<TData>,
    IAccessKey,
    ICharacterSlot,
    ITipRequest,
    ICredentials,
    ICombat,
    IDirection,
    ISequence
    where TData : IData
    where TAccount : IAccount<TMobile, TMobileCollection>
    where TMobile : IMobile
    where TMobileCollection : ICollection<TMobile>
{
    TMobile Character { get; }

    TAccount Account { get; }
}