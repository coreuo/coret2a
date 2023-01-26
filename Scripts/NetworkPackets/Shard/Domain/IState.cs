using Core.Abstract.Attributes;
using NetworkPackets.Shard.Features;
using NetworkPackets.Shared;
using NetworkPackets.Shared.Features;

namespace NetworkPackets.Shard.Domain;

[Entity("Shard", "State")]
public interface IState<TData, out TAccount, out TMobile, out TMobileCollection, TMap, TSkill, TSkillArray> : IState<TData>,
    IAccessKey,
    ICharacterCreate,
    ICharacterSlot,
    ITipRequest,
    IName,
    IPassword,
    ICombat,
    IDirection,
    ITarget,
    IStatus,
    IPattern,
    IMode,
    IExpansions,
    IPing,
    ICommand,
    IText,
    ILocation,
    IGender
    where TData : IData
    where TAccount : IAccount<TMobile, TMobileCollection>
    where TMobile : IMobile<TMap, TSkill, TSkillArray>
    where TMobileCollection : ICollection<TMobile>
    where TMap : IMap
    where TSkill : ISkill
    where TSkillArray : IReadOnlyList<TSkill>
{
    TMobile Character { get; }

    TAccount Account { get; }
}