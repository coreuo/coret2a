using Core.Abstract.Attributes;
using Packets.Shard.Features;

namespace Packets.Shard;

[Entity("Shard", "State")]
public interface IState<in TData, out TAccount, out TMobile, out TMobileCollection, TMap, TSkill, TSkillArray> : Server.IState<TData>,
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
    ICommand
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