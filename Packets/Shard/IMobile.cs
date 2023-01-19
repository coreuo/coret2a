using Core.Abstract.Attributes;
using Packets.Server.Features;
using Packets.Shard.Features;

namespace Packets.Shard;

[Entity("Mobile")]
public interface IMobile<TMap, TSkill, TSkillArray> : IMobile,
    ISkillList<TSkill, TSkillArray>
    where TMap : IMap
    where TSkill : ISkill
    where TSkillArray : IReadOnlyList<TSkill>
{
    TMap Map { get; }
}

[Entity("Mobile")]
public interface IMobile : 
    IName,
    IPassword,
    ISunlight,
    ILight,
    IWeather,
    IDirection,
    INotoriety,
    IStatus,
    IId,
    IBody,
    ILocation,
    IHue,
    IGender,
    IPoison,
    IBlessed,
    ICombat,
    IHidden,
    IHits,
    IMode,
    IStrength,
    IDexterity,
    IIntelligence,
    IStamina,
    IMana,
    IGold,
    IArmor,
    IWeight
{
}