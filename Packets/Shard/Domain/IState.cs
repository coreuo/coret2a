using Core.Abstract.Attributes;
using Packets.Shard.Features;
using Packets.Shared;
using Packets.Shared.Features;

namespace Packets.Shard.Domain;

[Entity("Shard", "State")]
public interface IState<TData, out TAccount, out TMobile, out TMobileCollection, TMap, TSkill, TSkillArray> : IState<TData>,
    IAccessKey,
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
    IText
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